using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class LevelEditorTimelineTrackGUI
    {
        private const float MarkerRadius = 6f;
        private const float DensityHeight = 200f;
        private const float DensityBottomMargin = 4f;
        private const float StackThresholdSeconds = 0.3f;
        private const float StackSpacingPixels = 20f;
        private const float RailYOffset = 50f;

        private bool isDraggingMarker = false;
        private int draggingIndex = -1;

        private bool isSelectingRect = false;
        private Vector2 selectStart;
        private Vector2 selectEnd;

        public void Draw(LevelEditorModel model, Rect rect)
        {
            if (Event.current.type == EventType.Repaint)
            {
                EditorGUI.DrawRect(rect, new Color(0.1f, 0.1f, 0.1f));
            }

            float total = Mathf.Max(1f, model.Duration);
            float secondsPerPixel = total / rect.width / model.TimelineZoom;
            float railY = rect.y + RailYOffset;

            DrawBaseline(rect, railY);
            DrawDensity(rect, model, secondsPerPixel);

            var stackOffsets = ComputeStackOffsets(model, StackThresholdSeconds, StackSpacingPixels);
            DrawMarkers(rect, model, secondsPerPixel, railY, stackOffsets);

            HandleMarkerDragging(rect, model, secondsPerPixel, total);
            DrawSelectionOverlay();
            FinalizeSelection(rect, model, secondsPerPixel, railY, stackOffsets);
            HandleBackgroundMouseDown(rect, model, secondsPerPixel, total);
        }

        private static void DrawBaseline(Rect rect, float railY)
        {
            Handles.BeginGUI();
            Handles.color = new Color(1f, 1f, 1f, 0.2f);
            Handles.DrawLine(new Vector3(rect.x, railY, 0), new Vector3(rect.xMax, railY, 0));
            Handles.EndGUI();
        }

        private void DrawDensity(Rect rect, LevelEditorModel model, float secondsPerPixel)
        {
            var densityRect = new Rect(rect.x, rect.yMax - DensityHeight - DensityBottomMargin, rect.width, DensityHeight);
            Handles.BeginGUI();
            Vector3? prev = null;
            for (int x = 0; x < densityRect.width; x += 4)
            {
                float t = model.TimelineScroll + x * secondsPerPixel;
                float count = CountAroundTime(model, t, 1f);
                float y = densityRect.yMax - (count / 5f) * densityRect.height;
                Vector3 curr = new Vector3(densityRect.x + x, y, 0);
                if (prev.HasValue)
                {
                    Handles.color = new Color(1f, 0.5f, 0f, 0.5f);
                    Handles.DrawLine(prev.Value, curr);
                }
                prev = curr;
            }
            Handles.EndGUI();
        }

        private void DrawMarkers(Rect rect, LevelEditorModel model, float secondsPerPixel, float railY, Dictionary<int, float> stackOffsets)
        {
            for (int i = 0; i < model.Meteors.Count; i++)
            {
                var m = model.Meteors[i];
                float x = TimeToPixel(m.spawnTime, rect, model, secondsPerPixel);
                var c = m.size switch
                {
                    MeteorSize.Large => new Color(0.95f, 0.55f, 0.55f),
                    MeteorSize.Medium => new Color(0.95f, 0.8f, 0.55f),
                    _ => new Color(0.6f, 0.8f, 1f)
                };

                float y = railY + (stackOffsets.TryGetValue(i, out var offset) ? offset : 0f);
                var markerRect = new Rect(x - MarkerRadius, y - MarkerRadius, MarkerRadius * 2, MarkerRadius * 2);

                EditorGUI.DrawRect(new Rect(markerRect.x, y - 1, markerRect.width, 2), c);
                EditorGUI.DrawRect(markerRect, c);

                bool isPrimary = i == model.SelectedIndex;
                bool isInGroup = model.SelectedSet != null && model.SelectedSet.Contains(i);
                if (isPrimary || isInGroup)
                {
                    Handles.BeginGUI();
                    Handles.color = isPrimary ? Color.cyan : Color.white;
                    Handles.DrawWireCube(markerRect.center, new Vector3(MarkerRadius * 2 + 4, MarkerRadius * 2 + 4, 0));
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

        private void HandleMarkerDragging(Rect rect, LevelEditorModel model, float secondsPerPixel, float total)
        {
            var e = Event.current;
            if (isDraggingMarker && draggingIndex >= 0 && draggingIndex < model.Meteors.Count)
            {
                if (e.type == EventType.MouseDrag)
                {
                    float t = model.TimelineScroll + (e.mousePosition.x - rect.x) * secondsPerPixel;
                    t = Mathf.Clamp(t, 0f, total);
                    model.Meteors[draggingIndex].spawnTime = t;
                    LevelEditorUtils.ClampSpawnTimes(model);
                    e.Use();
                }
                else if (e.type == EventType.MouseUp)
                {
                    isDraggingMarker = false;
                    draggingIndex = -1;
                    e.Use();
                }
            }
        }

        private void DrawSelectionOverlay()
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

        private void FinalizeSelection(Rect rect, LevelEditorModel model, float secondsPerPixel, float railY, Dictionary<int, float> stackOffsets)
        {
            if(model.CurrentViewFilter != ViewFilter.Group)
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
                float x = TimeToPixel(model.Meteors[i].spawnTime, rect, model, secondsPerPixel);
                float y = railY + (stackOffsets.TryGetValue(i, out var off) ? off : 0f);
                var markerRect = new Rect(x - MarkerRadius, y - MarkerRadius, MarkerRadius * 2, MarkerRadius * 2);

                if (selRect.Overlaps(markerRect) || selRect.Contains(markerRect.center))
                    model.SelectedSet.Add(i);
            }

            if (model.SelectedSet.Count > 0)
                model.CurrentViewFilter = ViewFilter.Group;

            Event.current.Use();
        }

        private void HandleBackgroundMouseDown(Rect rect, LevelEditorModel model, float secondsPerPixel, float total)
        {
            if (Event.current.type != EventType.MouseDown || !rect.Contains(Event.current.mousePosition)) return;

            if (LevelEditorUtils.IsCreateTool(model.CurrentTool))
            {
                float t = model.TimelineScroll + (Event.current.mousePosition.x - rect.x) * secondsPerPixel;
                t = Mathf.Clamp(t, 0f, total);
                var m = LevelEditorUtils.CreateMeteor(Vector3.zero, model.CurrentTool, t);
                model.Meteors.Add(m);
                model.SelectedIndex = model.Meteors.Count - 1;
                LevelEditorUtils.ClampSpawnTimes(model);
                Event.current.Use();
            }
            else if (model.CurrentViewFilter == ViewFilter.Group)
            {
                isSelectingRect = true;
                selectStart = selectEnd = Event.current.mousePosition;
                Event.current.Use();
            }
        }

        private static float CountAroundTime(LevelEditorModel model, float t, float range)
        {
            float count = 0f;
            for (int i = 0; i < model.Meteors.Count; i++)
            {
                if (Mathf.Abs(model.Meteors[i].spawnTime - t) <= range) count += 1f;
            }
            return count;
        }

        private static float TimeToPixel(float t, Rect rect, LevelEditorModel model, float secondsPerPixel)
        {
            return rect.x + (t - model.TimelineScroll) / secondsPerPixel;
        }

        private static Dictionary<int, float> ComputeStackOffsets(LevelEditorModel model, float threshold, float spacing)
        {
            var offsets = new Dictionary<int, float>();
            if (model.Meteors.Count == 0) return offsets;

            var list = new List<(int index, float time)>(model.Meteors.Count);
            for (int i = 0; i < model.Meteors.Count; i++)
            {
                list.Add((i, model.Meteors[i].spawnTime));
            }

            list.Sort((a, b) => a.time.CompareTo(b.time));
            int start = 0;
            while (start < list.Count)
            {
                int end = start + 1;
                float lastTime = list[start].time;
                var group = new List<int> { list[start].index };

                while (end < list.Count && list[end].time - lastTime <= threshold)
                {
                    group.Add(list[end].index);
                    lastTime = list[end].time;
                    end++;
                }

                int n = group.Count;
                if (n > 1)
                {
                    float mid = (n - 1) * 0.5f;
                    for (int k = 0; k < n; k++)
                    {
                        offsets[group[k]] = (k - mid) * spacing;
                    }
                }

                start = end;
            }
            return offsets;
        }
    }
}
