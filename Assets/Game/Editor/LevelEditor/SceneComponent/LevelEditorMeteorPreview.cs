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
            // grow pool
            for (int i = meteorGOs.Count; i < model.Meteors.Count; i++)
            {
                var prefab = GetPrefabFor(model.Meteors[i].size);
                GameObject go = prefab != null ? (GameObject)PrefabUtility.InstantiatePrefab(prefab) : new GameObject($"Meteor_{i+1}");
                if (go.GetComponent<SpriteRenderer>() == null) go.AddComponent<SpriteRenderer>();
                go.name = $"Meteor_{i+1}";
                go.transform.SetParent(meteorsRoot.transform);
                go.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy | HideFlags.NotEditable;
                foreach (var mb in go.GetComponentsInChildren<MonoBehaviour>()) mb.enabled = false;
                var rb = go.GetComponent<Rigidbody2D>(); if (rb) { rb.simulated = false; rb.isKinematic = true; }
                var col = go.GetComponent<Collider2D>(); if (col) col.enabled = false;
                meteorGOs.Add(go);
            }

            // shrink pool
            for (int i = meteorGOs.Count - 1; i >= model.Meteors.Count; i--)
            {
                if (meteorGOs[i] != null) Object.DestroyImmediate(meteorGOs[i]);
                meteorGOs.RemoveAt(i);
            }

            // update & replace if size changed
            for (int i = 0; i < meteorGOs.Count; i++)
            {
                var data = model.Meteors[i];
                var expectedPrefab = GetPrefabFor(data.size);
                var go = meteorGOs[i];
                bool needsReplace = go == null;
#if UNITY_EDITOR
                if (!needsReplace && expectedPrefab != null)
                {
                    var src = PrefabUtility.GetCorrespondingObjectFromSource(go);
                    if (src != expectedPrefab) needsReplace = true;
                }
#endif
                if (needsReplace)
                {
                    if (go != null) Object.DestroyImmediate(go);
                    go = expectedPrefab != null ? (GameObject)PrefabUtility.InstantiatePrefab(expectedPrefab) : new GameObject($"Meteor_{i+1}");
                    if (go.GetComponent<SpriteRenderer>() == null) go.AddComponent<SpriteRenderer>();
                    go.name = $"Meteor_{i+1}";
                    go.transform.SetParent(meteorsRoot.transform);
                    go.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy | HideFlags.NotEditable;
                    foreach (var mb in go.GetComponentsInChildren<MonoBehaviour>()) mb.enabled = false;
                    var rbNew = go.GetComponent<Rigidbody2D>(); if (rbNew) { rbNew.simulated = false; rbNew.isKinematic = true; }
                    var colNew = go.GetComponent<Collider2D>(); if (colNew) colNew.enabled = false;
                    meteorGOs[i] = go;
                }

                go.transform.position = new Vector3(data.position.x, data.position.y, 0f);
                go.SetActive(isVisible == null || isVisible(i));
            }
        }

        public void ClearMeteors()
        {
            for (int i = meteorGOs.Count - 1; i >= 0; i--)
            {
                if (meteorGOs[i] != null) Object.DestroyImmediate(meteorGOs[i]);
            }
            meteorGOs.Clear();
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
    }
}

