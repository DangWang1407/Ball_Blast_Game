using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class LevelEditorTimelineSelection
    {
        public void DrawOverlay(ref bool isSelectingRect, ref Vector2 selectStart, ref Vector2 selectEnd)
        {
            if (!isSelectingRect) return;

            var e2 = Event.current;
            if (e2.type == EventType.MouseDrag)
            {
                selectEnd = e2.mousePosition;
                e2.Use();
            }

            var min = Vector2.Min(selectStart, selectEnd);
            var max = Vector2.Max(selectStart, selectEnd);
            var selRect = new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
            EditorGUI.DrawRect(selRect, new Color(0.2f, 0.6f, 1f, 0.15f));
            Handles.BeginGUI();
            Handles.color = new Color(0.2f, 0.6f, 1f, 0.9f);
            Handles.DrawAAPolyLine(2f, new Vector3[]
            {
                new(selRect.xMin, selRect.yMin, 0),
                new(selRect.xMax, selRect.yMin, 0),
                new(selRect.xMax, selRect.yMax, 0),
                new(selRect.xMin, selRect.yMax, 0),
                new(selRect.xMin, selRect.yMin, 0)
            });
            Handles.EndGUI();
        }

        // Finalizes selection rectangle and updates model's selected set
        public void Finalize(Rect rect, LevelEditorModel model, float secondsPerPixel, float railY, Dictionary<int, float> stackOffsets, float markerRadius, ref bool isSelectingRect, Vector2 selectStart, Vector2 selectEnd)
        {
            if (model.CurrentViewFilter != ViewFilter.Group)
            {
                isSelectingRect = false;
                model.SelectedSet.Clear();
                return;
            }
            if (!(isSelectingRect && Event.current.type == EventType.MouseUp)) return;

            isSelectingRect = false;
            var min = Vector2.Min(selectStart, selectEnd);
            var max = Vector2.Max(selectStart, selectEnd);
            var selRect = new Rect(min.x, min.y, max.x - min.x, max.y - min.y);

            bool additive = Event.current.shift;
            if (!additive) model.SelectedSet.Clear();

            for (int i = 0; i < model.Meteors.Count; i++)
            {
                float x = LevelEditorTimelineMath.TimeToPixel(model.Meteors[i].spawnTime, rect, model, secondsPerPixel);
                float y = railY + (stackOffsets.TryGetValue(i, out var off) ? off : 0f);
                var markerRect = new Rect(x - markerRadius, y - markerRadius, markerRadius * 2, markerRadius * 2);

                if (selRect.Overlaps(markerRect) || selRect.Contains(markerRect.center))
                    model.SelectedSet.Add(i);
            }

            if (model.SelectedSet.Count > 0)
                model.CurrentViewFilter = ViewFilter.Group;

            Event.current.Use();
        }
    }
}
