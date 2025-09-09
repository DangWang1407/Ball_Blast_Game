using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class LevelEditorWindow : EditorWindow
    {
        [SerializeField] private LevelEditorModel model;
        private Vector2 scrollPos;

        [MenuItem("Window/Level Editor")]
        public static void OpenWindow()
        {
            GetWindow<LevelEditorWindow>("Level Editor").Show();
        }

        private void OnEnable()
        {
            if (model == null)
            {
                model = new LevelEditorModel();
            }
            LevelEditorSceneGUI.Attach(model, Repaint);
        }

        private void OnDisable()
        {
            LevelEditorSceneGUI.Detach();
        }

        private void OnGUI()
        {
            GUILayout.Label("Level Editor", EditorStyles.boldLabel);
            LevelEditorToolbarGUI.Draw(model, OnClickLoad, OnClickSave);
            GUILayout.Space(6);

            // Only Timeline view
            float timelineHeight = Mathf.Clamp(position.height * 0.5f, 220f, 340f);
            Rect timelineRect = GUILayoutUtility.GetRect(position.width - 12, timelineHeight);
            LevelEditorTimelineGUI.Draw(model, timelineRect);

            GUILayout.Space(6);
            LevelEditorListGUI.Draw(model, ref scrollPos);
        }

        private void OnClickLoad() => LevelEditorIO.Load(model, Repaint);
        private void OnClickSave() => LevelEditorIO.Save(model);
    }
}
