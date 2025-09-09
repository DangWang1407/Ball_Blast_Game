using System.IO;
using Game.Controllers;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Game.Editor
{
    public class LevelEditorPlayRunner
    {

        private const string PlaytestResName = "_playtest";
        private const string PlaytestResKey = "LevelEditorPlaytest_Respath";
        private const string PreviousScenekey = "LevelEditor_PreviousScenePath";


        public void PlayFromEditor(LevelEditorModel model)
        {
            if (model == null) return;

            string resourceDir = Path.Combine(Application.dataPath, "Resources");
            if (!Directory.Exists(resourceDir))
            {
                Directory.CreateDirectory(resourceDir);
            }

            string jsonPath = Path.Combine(resourceDir, PlaytestResName + ".json");

            var save = new MeteorSpawnList { meteors = model.Meteors.ToArray() };
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
                if (active.IsValid())
                {
                    EditorPrefs.SetString(PreviousScenekey, active.path);
                }

                var startScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(targetScene);
                EditorSceneManager.playModeStartScene = startScene;
            }

            EditorApplication.isPlaying = true;
        }

        private string FindGameplayScenePath()
        {
            // Maybe replace with Init scene 
            return "Assets/Scenes/GamePlay.unity";
        }

        [InitializeOnLoadMethod]
        private static void HookPlayMode()
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
                string prev = EditorPrefs.GetString(PreviousScenekey, string.Empty);
                if (!string.IsNullOrEmpty(prev) && File.Exists(Path.Combine(Directory.GetCurrentDirectory(), prev)))
                {
                    EditorSceneManager.OpenScene(prev);
                }
                EditorPrefs.DeleteKey(PreviousScenekey);

                // Clear playmode start scene
                EditorSceneManager.playModeStartScene = null;
            }
        }
    }
}