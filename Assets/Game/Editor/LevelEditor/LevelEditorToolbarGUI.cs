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

            // Tool mode and duration
            EditorGUILayout.BeginHorizontal();
            model.CurrentTool = (ToolMode)EditorGUILayout.EnumPopup("Tool", model.CurrentTool, GUILayout.MaxWidth(300));
            EditorGUILayout.EndHorizontal();

            // View filter
            EditorGUILayout.Space(4);
            EditorGUILayout.BeginHorizontal();
            model.CurrentViewFilter = (ViewFilter)EditorGUILayout.EnumPopup("View", model.CurrentViewFilter, GUILayout.MaxWidth(300));
            if (model.CurrentViewFilter == ViewFilter.Group)
            {
                GUILayout.Space(6); GUILayout.Label($"Selected count: {model.SelectedSet.Count}");
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            // Load / Save / Play
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

            // Asset path
            GUILayout.Space(4);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Asset Path:", GUILayout.MaxWidth(70));
            GUILayout.Label(string.IsNullOrEmpty(model.CurrentAssetPath) ? "<none>" : model.CurrentAssetPath);

            GUILayout.Space(4);
            model.Duration = EditorGUILayout.FloatField("Duration (s)", model.Duration, GUILayout.MaxWidth(200));

            EditorGUILayout.EndHorizontal();

            GUILayout.EndArea();
        }
    }
}