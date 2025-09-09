using UnityEditor;
using UnityEngine;
using Game.Controllers;

namespace Game.Editor
{
    public static class LevelEditorListGUI
    {
        public static void Draw(LevelEditorModel model, ref Vector2 scrollPos)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();
            model.ShowList = EditorGUILayout.Foldout(model.ShowList, "Meteor Info", true);
            GUILayout.FlexibleSpace();
            using (new EditorGUI.DisabledScope(model.SelectedIndex < 0 || model.SelectedIndex >= model.Meteors.Count))
            {
                if (GUILayout.Button("Deselect", GUILayout.Width(80)))
                {
                    model.SelectedIndex = -1;
                }
            }
            EditorGUILayout.EndHorizontal();

            // Clamp to duration
            LevelEditorUtils.ClampSpawnTimes(model);

            if (!model.ShowList)
            {
                EditorGUILayout.EndVertical();
                return;
            }

            // Show only the selected meteor's info
            if (model.SelectedIndex >= 0 && model.SelectedIndex < model.Meteors.Count)
            {
                var m = model.Meteors[model.SelectedIndex];
                EditorGUILayout.BeginVertical("box");
                GUILayout.Label($"Selected Meteor #{model.SelectedIndex + 1}", EditorStyles.boldLabel);
                EditorGUILayout.Space(4);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Size", GUILayout.Width(60));
                GUILayout.Label(m.size.ToString(), GUILayout.Width(120));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(4);

                var newPos = EditorGUILayout.Vector3Field("Position", m.position);
                if (newPos != m.position) m.position = newPos;
                EditorGUILayout.Space(4);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Health", GUILayout.Width(60));
                m.health = Mathf.Max(1, EditorGUILayout.IntField(m.health, GUILayout.Width(80)));
                EditorGUILayout.Space(4);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(4);

                GUILayout.Label("Spawn time");
                m.spawnTime = EditorGUILayout.Slider(m.spawnTime, 0f, Mathf.Max(0.0001f, model.Duration), GUILayout.MaxWidth(400));

                EditorGUILayout.Space(6);
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Delete", GUILayout.Width(90)))
                {
                    model.Meteors.RemoveAt(model.SelectedIndex);
                    model.SelectedIndex = -1;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndVertical();
                    return;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            else
            {
                if (model.Meteors.Count == 0)
                {
                    EditorGUILayout.HelpBox("No meteors yet. Choose a create tool and click in Scene.", MessageType.Info);
                }
                else
                {
                    EditorGUILayout.HelpBox("No meteor selected. Select one from the Timeline or Scene.", MessageType.Info);
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}
