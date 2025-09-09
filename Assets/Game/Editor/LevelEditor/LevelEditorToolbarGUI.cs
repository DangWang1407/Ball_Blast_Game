using System;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public static class LevelEditorToolbarGUI
    {
        public static void Draw(LevelEditorModel model, Action onLoad, Action onSave)
        {
            EditorGUILayout.BeginHorizontal();
            model.CurrentTool = (ToolMode)EditorGUILayout.EnumPopup("Tool", model.CurrentTool, GUILayout.MaxWidth(300));
            GUILayout.Space(6);
            model.Duration = EditorGUILayout.FloatField("Duration", Mathf.Max(0f, model.Duration), GUILayout.MaxWidth(320));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(4);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Load", GUILayout.MaxWidth(90))) onLoad?.Invoke();
            if (GUILayout.Button("Save", GUILayout.MaxWidth(90))) onSave?.Invoke();
            GUILayout.Space(6);
            string playLabel = Application.isPlaying ? "Stop" : "Play";
            if (GUILayout.Button(playLabel, GUILayout.MaxWidth(90)))
            {
                if (Application.isPlaying)
                    UnityEditor.EditorApplication.isPlaying = false;
                else
                    LevelEditorPlayRunner.PlayFromEditor(model);
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            // Current file path
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("File:", GUILayout.Width(30));
            GUILayout.Label(string.IsNullOrEmpty(model.CurrentAssetPath) ? "<new level>" : model.CurrentAssetPath);
            EditorGUILayout.EndHorizontal();
        }
    }
}
