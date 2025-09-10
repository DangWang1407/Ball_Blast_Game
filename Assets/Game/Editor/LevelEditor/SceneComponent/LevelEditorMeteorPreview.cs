using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Game.Controllers;

namespace Game.Editor
{
    public class LevelEditorMeteorPreview
    {
        private const string LargeMeteorPrefabPath = "Assets/Game/Prefabs/Meteors/Large_2.prefab";
        private const string MediumMeteorPrefabPath = "Assets/Game/Prefabs/Meteors/Medium_2.prefab";
        private const string SmallMeteorPrefabPath = "Assets/Game/Prefabs/Meteors/Small_2.prefab";

        private GameObject meteorsRoot;
        private GameObject prefabLarge, prefabMedium, prefabSmall;
        private readonly List<GameObject> meteorGOs = new List<GameObject>();
        private readonly List<MeteorSize> lastSizes = new List<MeteorSize>();

        public void EnsurePrefabs()
        {
            if (prefabLarge == null) prefabLarge = AssetDatabase.LoadAssetAtPath<GameObject>(LargeMeteorPrefabPath);
            if (prefabMedium == null) prefabMedium = AssetDatabase.LoadAssetAtPath<GameObject>(MediumMeteorPrefabPath);
            if (prefabSmall == null) prefabSmall = AssetDatabase.LoadAssetAtPath<GameObject>(SmallMeteorPrefabPath);
        }

        public void EnsureMeteorsRoot()
        {
            if (meteorsRoot != null) return;
            meteorsRoot = new GameObject("LevelEditorMeteors");
            meteorsRoot.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy | HideFlags.NotEditable;
        }

        public void SyncMeteorInstances(LevelEditorModel model, System.Func<int, bool> isVisible)
        {
            if (meteorsRoot == null) EnsureMeteorsRoot();
            // grow pool
            for (int i = meteorGOs.Count; i < model.Meteors.Count; i++)
            {
                var prefab = GetPrefabFor(model.Meteors[i].size);
                GameObject go = prefab != null ? (GameObject)PrefabUtility.InstantiatePrefab(prefab) : new GameObject($"Meteor_{i+1}");
                SetupPreviewGameObject(go, i);
                meteorGOs.Add(go);
                lastSizes.Add(model.Meteors[i].size);
            }

            // shrink pool
            for (int i = meteorGOs.Count - 1; i >= model.Meteors.Count; i--)
            {
                if (meteorGOs[i] != null) Object.DestroyImmediate(meteorGOs[i]);
                meteorGOs.RemoveAt(i);
                if (i < lastSizes.Count) lastSizes.RemoveAt(i);
            }

            // update & replace if size changed
            for (int i = 0; i < meteorGOs.Count; i++)
            {
                var data = model.Meteors[i];
                var go = meteorGOs[i];

                if (i >= lastSizes.Count) lastSizes.Add(data.size);

                if (go == null || lastSizes[i] != data.size)
                {
                    var expectedPrefab = GetPrefabFor(data.size);
                    if (go != null) Object.DestroyImmediate(go);
                    go = expectedPrefab != null ? (GameObject)PrefabUtility.InstantiatePrefab(expectedPrefab) : new GameObject($"Meteor_{i+1}");
                    SetupPreviewGameObject(go, i);
                    meteorGOs[i] = go;
                    lastSizes[i] = data.size;
                }

                // keep name consistent with index after any deletion
                if (go.name != $"Meteor_{i+1}") go.name = $"Meteor_{i+1}";

                var newPos = new Vector3(data.position.x, data.position.y, 0f);
                if (go.transform.position != newPos) go.transform.position = newPos;
                bool shouldActive = isVisible == null || isVisible(i);
                if (go.activeSelf != shouldActive) go.SetActive(shouldActive);
            }
        }

        public void ClearMeteors()
        {
            for (int i = meteorGOs.Count - 1; i >= 0; i--)
            {
                if (meteorGOs[i] != null) Object.DestroyImmediate(meteorGOs[i]);
            }
            meteorGOs.Clear();
            lastSizes.Clear();
            if (meteorsRoot != null)
            {
                Object.DestroyImmediate(meteorsRoot);
                meteorsRoot = null;
            }
        }

        private GameObject GetPrefabFor(MeteorSize size)
        {
            return size switch
            {
                MeteorSize.Large => prefabLarge,
                MeteorSize.Medium => prefabMedium,
                _ => prefabSmall
            };
        }

        private void SetupPreviewGameObject(GameObject go, int index)
        {
            if (go.GetComponent<SpriteRenderer>() == null) go.AddComponent<SpriteRenderer>();
            go.name = $"Meteor_{index + 1}";
            go.transform.SetParent(meteorsRoot.transform);
            go.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy | HideFlags.NotEditable;
            foreach (var mb in go.GetComponentsInChildren<MonoBehaviour>()) mb.enabled = false;
            var rb = go.GetComponent<Rigidbody2D>(); if (rb) { rb.simulated = false; rb.isKinematic = true; }
            var col = go.GetComponent<Collider2D>(); if (col) col.enabled = false;
        }
    }
}
