using UnityEditor;
using UnityEngine;
using Game.Controllers;

namespace Game.Editor
{
    public class LevelEditorInfoGUI
    {
        public void Draw(LevelEditorModel model, Rect rect)
        {
            if (model == null) return;

            GUILayout.BeginArea(rect);
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Meteor Info", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            using (new EditorGUI.DisabledScope(model.SelectedIndex < 0 || model.SelectedIndex >= model.Meteors.Count))
            {
                if (GUILayout.Button("Deselect", GUILayout.Width(90)))
                {
                    model.SelectedIndex = -1;
                }
            }
            EditorGUILayout.EndHorizontal();

            if (model.SelectedIndex >= 0 && model.SelectedIndex < model.Meteors.Count)
            {
                var m = model.Meteors[model.SelectedIndex];

                //Index
                EditorGUILayout.LabelField("Index #" + model.SelectedIndex.ToString());

                // Position
                var newPos = EditorGUILayout.Vector3Field("Position", m.position);
                if (newPos != m.position) m.position = newPos;

                // Size
                GUILayout.Space(10);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Size", GUILayout.Width(60));
                var newSize = (MeteorSize)EditorGUILayout.EnumPopup(m.size, GUILayout.Width(160));
                if (newSize != m.size) m.size = newSize;
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                // Health
                GUILayout.Space(10);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Health", GUILayout.Width(60));
                m.health = Mathf.Max(1, EditorGUILayout.IntField(m.health, GUILayout.Width(50)));
                EditorGUILayout.EndHorizontal();

                // Spawn time
                GUILayout.Space(10);
                GUILayout.Label("Spawn time");
                EditorGUILayout.BeginHorizontal();
                float maxTime = Mathf.Max(0.0001f, model.Duration);
                m.spawnTime = EditorGUILayout.Slider(m.spawnTime, 0f, maxTime, GUILayout.MaxWidth(400));
                // m.spawnTime = Mathf.Clamp(EditorGUILayout.FloatField(m.spawnTime, GUILayout.Width(80)), 0f, maxTime);
                EditorGUILayout.EndHorizontal();

                // Delete
                EditorGUILayout.Space(6);
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Delete", GUILayout.Width(90)))
                {
                    model.Meteors.RemoveAt(model.SelectedIndex);
                    model.SelectedIndex = -1;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    return;
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.HelpBox("No meteor selected. Select one from the Timeline or Scene.", MessageType.Info);
            }

            EditorGUILayout.EndVertical();

            GUILayout.EndArea();
        }
    }
}

