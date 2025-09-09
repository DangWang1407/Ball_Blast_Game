using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Game.Editor
{
    public static class LevelEditorPlayRunner
    {
        private const string PlaytestResName = "__playtest"; // Resources/__playtest.json
        private const string PlaytestResKey = "LevelEditorPlaytest_ResPath";
        private const string PreviousSceneKey = "LevelEditor_PreviousScenePath";

        public static void PlayFromEditor(LevelEditorModel model)
        {
            if (model == null) return;

            string resourcesDir = Path.Combine(Application.dataPath, "Resources");
            if (!Directory.Exists(resourcesDir)) Directory.CreateDirectory(resourcesDir);
            string jsonPath = Path.Combine(resourcesDir, PlaytestResName + ".json");

            var save = new Game.Controllers.MeteorSpawnList { meteors = model.Meteors.ToArray() };
            string json = JsonUtility.ToJson(save, true);
            File.WriteAllText(jsonPath, json);

            string projPath = "Assets/Resources/" + PlaytestResName + ".json";
            AssetDatabase.ImportAsset(projPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            PlayerPrefs.SetString(PlaytestResKey, PlaytestResName);
            PlayerPrefs.Save();

            string targetScene = FindGameplayScenePath();
            if (!string.IsNullOrEmpty(targetScene))
            {
                var active = EditorSceneManager.GetActiveScene();
                if (active.IsValid()) EditorPrefs.SetString(PreviousSceneKey, active.path);

                var startScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(targetScene);
                EditorSceneManager.playModeStartScene = startScene;
            }

            // 4) Enter Play Mode
            EditorApplication.isPlaying = true;
        }

        private static string FindGameplayScenePath()
        {
            // Prefer Assets/Scenes/GamePlay.unity
            string p1 = "Assets/Scenes/GamePlay.unity";
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(p1) != null) return p1;
            // Fallback: Assets/Scenes/Init.unity
            string p2 = "Assets/Scenes/Init.unity";
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(p2) != null) return p2;
            // Otherwise keep current
            return string.Empty;
        }

        [InitializeOnLoadMethod]
        private static void HookPlaymode()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private static void OnPlayModeChanged(PlayModeStateChange change)
        {
            if (change == PlayModeStateChange.EnteredEditMode)
            {
                // Clear override
                PlayerPrefs.DeleteKey(PlaytestResKey);
                PlayerPrefs.Save();

                // Restore previous edit scene if stored
                string prev = EditorPrefs.GetString(PreviousSceneKey, string.Empty);
                if (!string.IsNullOrEmpty(prev) && File.Exists(Path.Combine(Directory.GetCurrentDirectory(), prev)))
                {
                    EditorSceneManager.OpenScene(prev);
                }
                EditorPrefs.DeleteKey(PreviousSceneKey);

                // Clear playmode start scene
                EditorSceneManager.playModeStartScene = null;
            }
        }
    }
}

