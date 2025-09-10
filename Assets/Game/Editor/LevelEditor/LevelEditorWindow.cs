using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class LevelEditorWindow : EditorWindow
    {
        private LevelEditorModel model;
        private LevelEditorToolbarGUI toolbarGUI;
        private LevelEditorTimelineGUI timelineGUI;
        private LevelEditorIO io;
        private LevelEditorSceneGUI sceneGUI;
        private LevelEditorInfoGUI infoGUI;

        [MenuItem("Window/Level Editor")]
        public static void OpenWindow()
        {
            GetWindow<LevelEditorWindow>("Level Editor").Show();
        }

        private void OnEnable()
        {
            model ??= new LevelEditorModel();
            toolbarGUI ??= new LevelEditorToolbarGUI();
            timelineGUI ??= new LevelEditorTimelineGUI();
            io ??= new LevelEditorIO();
            sceneGUI ??= new LevelEditorSceneGUI();
            infoGUI ??= new LevelEditorInfoGUI();
            
            sceneGUI.Attach(model, Repaint);
        }

        private void OnDisable()
        {
            sceneGUI.Detach();
        }

        private void OnGUI()
        {
            // GUILayout.Label("Level Editor", EditorStyles.boldLabel);
            Rect toolbarRect = new Rect(10, 10, position.width - 20, 100);
            toolbarGUI.Draw(model, OnLoad, OnSave, toolbarRect);
            GUILayout.Space(4);

            float timelineHeight = Mathf.Clamp(position.height * 0.5f, 220f, 340f);
            Rect timelineRect = new Rect(10, toolbarRect.yMax + 6, position.width - 20, timelineHeight);
            timelineGUI.Draw(model, timelineRect);

            GUILayout.Space(6);
            Rect infoRect = new Rect(10, timelineRect.yMax + 6, position.width - 20, position.height - timelineRect.yMax - 16);
            infoGUI.Draw(model, infoRect);
        }

        private void OnLoad() => io.Load(model, Repaint);
        private void OnSave() => io.Save(model);
    }
}