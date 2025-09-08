using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class LevelEditorWindow : EditorWindow
    {
        [MenuItem("Window/Level Editor")]
        public static void OpenWindow()
        {
            LevelEditorWindow window = GetWindow<LevelEditorWindow>("Level Editor");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Level Editor", EditorStyles.boldLabel);
        }
    }
}
