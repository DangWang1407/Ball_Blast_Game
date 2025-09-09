using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Game.Controllers;

namespace Game.Editor
{
    public static class LevelEditorIO
    {
        private const string LevelConfigsDir = "Assets/Game/Scripts/LevelConfigs";

        private static string ProjectToAbsolute(string projectPath)
        {
            // projectPath like "Assets/..."
            return System.IO.Path.Combine(Application.dataPath, projectPath.Substring("Assets/".Length));
        }

        public static void Save(LevelEditorModel model)
        {
#if UNITY_EDITOR
            // Ensure folder exists
            var dirAbs = ProjectToAbsolute(LevelConfigsDir);
            if (!Directory.Exists(dirAbs)) Directory.CreateDirectory(dirAbs);

            // Keep list ordered by spawn time for consistency
            model.Meteors.Sort((a,b) => a.spawnTime.CompareTo(b.spawnTime));
            var save = new MeteorSpawnList { meteors = model.Meteors.ToArray() };
            string json = JsonUtility.ToJson(save, true);

            // Save to current file if available; otherwise create a new unique one
            string projectPath = string.IsNullOrEmpty(model.CurrentAssetPath)
                ? AssetDatabase.GenerateUniqueAssetPath($"{LevelConfigsDir}/level.json")
                : model.CurrentAssetPath;

            // Convert to absolute path and write
            string abs = ProjectToAbsolute(projectPath);
            File.WriteAllText(abs, json);

            AssetDatabase.ImportAsset(projectPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            model.CurrentAssetPath = projectPath;
#endif
        }

        public static void Load(LevelEditorModel model, Action repaint)
        {
#if UNITY_EDITOR
            string dirAbs = ProjectToAbsolute(LevelConfigsDir);
            if (!Directory.Exists(dirAbs)) Directory.CreateDirectory(dirAbs);
            string path = EditorUtility.OpenFilePanel("Load Level JSON", dirAbs, "json");
            if (string.IsNullOrEmpty(path)) return;
            var normPath = path.Replace('\\','/');
            var normData = Application.dataPath.Replace('\\','/');
            if (!normPath.StartsWith(normData))
            {
                EditorUtility.DisplayDialog("Invalid File", "Pick a file inside the project's Assets folder.", "OK");
                return;
            }
            string projectPath = "Assets" + normPath.Substring(normData.Length);
            var ta = AssetDatabase.LoadAssetAtPath<TextAsset>(projectPath);
            if (ta == null) return;
            var data = JsonUtility.FromJson<MeteorSpawnList>(ta.text);
            if (data == null || data.meteors == null) return;

            model.Meteors.Clear();
            float maxTime = 0f;
            foreach (var m in data.meteors)
            {
                model.Meteors.Add(new MeteorData { position = m.position, size = m.size, health = m.health, spawnTime = m.spawnTime });
                if (m.spawnTime > maxTime) maxTime = m.spawnTime;
            }
            model.Duration = Mathf.Max(model.Duration, maxTime);
            model.CurrentAssetPath = projectPath;
            repaint?.Invoke();
            SceneView.RepaintAll();
#endif
        }
    }
}
