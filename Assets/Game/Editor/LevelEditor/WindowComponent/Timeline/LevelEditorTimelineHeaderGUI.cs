using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class LevelEditorTimelineHeaderGUI
    {
        public void Draw(LevelEditorModel model, Rect rect)
        {
            GUILayout.BeginArea(rect);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Timeline", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            EditorGUIUtility.labelWidth = 50f;
            EditorGUILayout.Space(40);
            model.TimelineZoom = Mathf.Max(0.25f, EditorGUILayout.Slider("Zoom", model.TimelineZoom, 0.25f, 5f, GUILayout.MaxWidth(220)));
            EditorGUIUtility.labelWidth = 0f;
            EditorGUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }
}

