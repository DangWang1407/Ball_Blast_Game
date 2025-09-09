using System;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class LevelEditorToolbarGUI
    {
        private LevelEditorPlayRunner playRunner;
        public LevelEditorToolbarGUI()
        {
            playRunner = new LevelEditorPlayRunner();
        }

        public void Draw(LevelEditorModel model, Action onLoad, Action onSave, Rect rect)
        {
            GUILayout.BeginArea(rect);

            EditorGUILayout.BeginHorizontal();
            model.CurrentTool = (ToolMode)EditorGUILayout.EnumPopup("Tool", model.CurrentTool, GUILayout.MaxWidth(300));
            GUILayout.Space(6);
            model.Duration = EditorGUILayout.FloatField("Duration (s)", model.Duration, GUILayout.MaxWidth(200));
            // GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(4);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Load", GUILayout.MaxWidth(90))) onLoad?.Invoke();
            if (GUILayout.Button("Save", GUILayout.MaxWidth(90))) onSave?.Invoke();
            GUILayout.Space(6);
            string playLabel = Application.isPlaying ? "Stop" : "Play";
            if (GUILayout.Button(playLabel, GUILayout.MaxWidth(90)))
            {
                if (Application.isPlaying) EditorApplication.isPlaying = false;
                else playRunner.PlayFromEditor(model);
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(4);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Asset Path:", GUILayout.MaxWidth(70));
            GUILayout.Label(string.IsNullOrEmpty(model.CurrentAssetPath) ? "<none>" : model.CurrentAssetPath);
            EditorGUILayout.EndHorizontal();

            GUILayout.EndArea();
        }
    }
}