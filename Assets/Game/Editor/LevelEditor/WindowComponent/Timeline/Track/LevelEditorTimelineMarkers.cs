using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    // Draws and manages timeline markers for meteors
    public class LevelEditorTimelineMarkers
    {
        public void Draw(Rect rect, LevelEditorModel model, float secondsPerPixel, float railY, Dictionary<int, float> stackOffsets, float markerRadius, ref bool isDraggingMarker, ref int draggingIndex)
        {
            for (int i = 0; i < model.Meteors.Count; i++)
            {
                var m = model.Meteors[i];
                float x = LevelEditorTimelineMath.TimeToPixel(m.spawnTime, rect, model, secondsPerPixel);
                var c = m.size switch
                {
                    MeteorSize.Large => new Color(0.95f, 0.55f, 0.55f),
                    MeteorSize.Medium => new Color(0.95f, 0.8f, 0.55f),
                    _ => new Color(0.6f, 0.8f, 1f)
                };

                float y = railY + (stackOffsets.TryGetValue(i, out var offset) ? offset : 0f);
                var markerRect = new Rect(x - markerRadius, y - markerRadius, markerRadius * 2, markerRadius * 2);

                EditorGUI.DrawRect(new Rect(markerRect.x, y - 1, markerRect.width, 2), c);
                EditorGUI.DrawRect(markerRect, c);

                bool isPrimary = i == model.SelectedIndex;
                bool isInGroup = model.SelectedSet != null && model.SelectedSet.Contains(i);
                if (isPrimary || isInGroup)
                {
                    Handles.BeginGUI();
                    Handles.color = isPrimary ? Color.cyan : Color.white;
                    Handles.DrawWireCube(markerRect.center, new Vector3(markerRadius * 2 + 4, markerRadius * 2 + 4, 0));
                    Handles.EndGUI();
                }

                var evt = Event.current;
                if (evt.type == EventType.MouseDown && evt.button == 0 && markerRect.Contains(evt.mousePosition))
                {
                    model.SelectedIndex = i;
                    isDraggingMarker = true;
                    draggingIndex = i;
                    evt.Use();
                }
            }
        }
    }
}
