using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class LevelEditorSceneBackground
    {
        private const string BackgroundPrefabPath = "Assets/Game/Prefabs/Background/Background.prefab";
        private GameObject backgroundGo;

        public void EnsureBackground()
        {
            if (backgroundGo != null) return;
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(BackgroundPrefabPath);
            if (prefab == null) return;
            var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (go == null) return;
            go.name = "LevelEditorBackground";
            go.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
            var pos = go.transform.position;
            go.transform.position = new Vector3(pos.x, pos.y, 1f);
            foreach (var mb in go.GetComponentsInChildren<MonoBehaviour>()) mb.enabled = false;
            backgroundGo = go;
        }

        public void ClearBackground()
        {
            if (backgroundGo == null) return;
            Object.DestroyImmediate(backgroundGo);
            backgroundGo = null;
        }
    }
}

