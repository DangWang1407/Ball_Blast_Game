using System;
using UnityEditor;
using UnityEngine;
using Game.Controllers;

namespace Game.Editor
{
    public static class LevelEditorTimelineGUI
    {
        private const float HeaderHeight = 22f;
        private const float RulerHeight = 24f;
        private const float TrackHeight = 200f; // visual rail + small density area
        private const float MarkerRadius = 6f;
        private const float DensityHeight = 200f; // height of the mini-graph
        private const float DensityBottomMargin = 4f; // space from bottom edge

        private static bool isDraggingMarker = false;
        private static int draggingIndex = -1;

        public static void Draw(LevelEditorModel model, Rect rect)
        {
            if (Event.current.type == EventType.Repaint)
            {
                EditorGUI.DrawRect(rect, new Color(0.12f, 0.12f, 0.12f));
            }

            // Header
            var header = new Rect(rect.x, rect.y, rect.width, HeaderHeight);
            DrawHeader(model, header);

            // Ruler
            var ruler = new Rect(rect.x, header.yMax, rect.width, RulerHeight);
            DrawRuler(model, ruler);

            // Track (compact)
            float available = Mathf.Max(32f, rect.height - HeaderHeight - RulerHeight - 8);
            var track = new Rect(rect.x + 8, ruler.yMax + (available - TrackHeight) * 0.5f, rect.width - 16, Mathf.Min(TrackHeight, available));
            DrawTrack(model, track);
        }

        private static void DrawHeader(LevelEditorModel model, Rect rect)
        {
            GUILayout.BeginArea(rect);
            EditorGUILayout.BeginHorizontal();
            // GUILayout.Label("Timeline", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            EditorGUIUtility.labelWidth = 50f;
            EditorGUILayout.Space(40);
            model.TimelineZoom = Mathf.Max(0.25f, EditorGUILayout.Slider("Zoom", model.TimelineZoom, 0.25f, 5f, GUILayout.MaxWidth(220)));
            EditorGUIUtility.labelWidth = 0f;
            EditorGUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private static void DrawRuler(LevelEditorModel model, Rect rect)
        {
            if (Event.current.type == EventType.Repaint)
            {
                EditorGUI.DrawRect(rect, new Color(0.16f, 0.16f, 0.16f));
            }
            // Compute visible range
            float total = Mathf.Max(0.0001f, model.Duration);
            float secondsPerPixel = total / rect.width / model.TimelineZoom;
            float visibleSpan = secondsPerPixel * rect.width;
            model.TimelineScroll = Mathf.Clamp(model.TimelineScroll, 0f, Mathf.Max(0f, total - visibleSpan));

            Handles.BeginGUI();
            Handles.color = new Color(1f, 1f, 1f, 0.2f);
            Handles.DrawLine(new Vector3(rect.x, rect.yMax, 0), new Vector3(rect.xMax, rect.yMax, 0));

            // Major ticks every N seconds based on zoom
            float[] candidates = { 0.1f, 0.25f, 0.5f, 1f, 2f, 5f, 10f };
            float minPx = 60f;
            float step = 1f;
            foreach (var c in candidates)
            {
                if (c / secondsPerPixel >= minPx) { step = c; break; }
            }

            GUIStyle tickStyle = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.UpperCenter };
            for (float t = Mathf.Floor(model.TimelineScroll / step) * step; t <= model.TimelineScroll + visibleSpan + 0.001f; t += step)
            {
                float x = TimeToPixel(t, rect, model, secondsPerPixel);
                Handles.color = new Color(1f, 1f, 1f, 0.25f);
                Handles.DrawLine(new Vector3(x, rect.y + 6, 0), new Vector3(x, rect.yMax, 0));
                GUI.Label(new Rect(x - 30, rect.y + 2, 60, 16), t.ToString("0.##s"), tickStyle);
            }
            Handles.EndGUI();

            // Scrollbar
            float scrollPixels = model.TimelineScroll / secondsPerPixel;
            float viewPixels = visibleSpan / secondsPerPixel;
            float totalPixels = total / secondsPerPixel;
            var scrollRect = new Rect(rect.x + 6, rect.yMax - 8, rect.width - 12, 6);
            EditorGUI.MinMaxSlider(scrollRect, ref scrollPixels, ref viewPixels, 0f, totalPixels);
            model.TimelineScroll = scrollPixels * secondsPerPixel;
        }

        private static void DrawTrack(LevelEditorModel model, Rect rect)
        {
            if (Event.current.type == EventType.Repaint)
            {
                EditorGUI.DrawRect(rect, new Color(0.10f, 0.10f, 0.10f));
            }

            float total = Mathf.Max(0.0001f, model.Duration);
            float secondsPerPixel = total / (rect.width) / model.TimelineZoom;

            // Baseline rail
            float railY = rect.y + 24f;
            Handles.BeginGUI();
            Handles.color = new Color(1f, 1f, 1f, 0.15f);
            Handles.DrawLine(new Vector3(rect.x, railY, 0), new Vector3(rect.xMax, railY, 0));
            Handles.EndGUI();

            // Density mini-graph at bottom
            var densityRect = new Rect(rect.x, rect.yMax - (DensityHeight + DensityBottomMargin), rect.width, DensityHeight);
            Handles.BeginGUI();
            Vector3? prev = null;
            for (int x = 0; x < densityRect.width; x += 4)
            {
                float t = model.TimelineScroll + x * secondsPerPixel;
                float density = CountAroundTime(model, t, 1f) / 10f; // normalized
                float y = Mathf.Lerp(densityRect.yMax, densityRect.yMin, Mathf.Clamp01(density));
                Vector3 p = new Vector3(densityRect.x + x, y, 0);
                if (prev.HasValue) Handles.DrawLine(prev.Value, p);
                prev = p;
            }
            
            Handles.EndGUI();

            // Markers
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

                var markerRect = new Rect(x - MarkerRadius, railY - MarkerRadius, MarkerRadius * 2, MarkerRadius * 2);
                EditorGUI.DrawRect(new Rect(markerRect.x, railY - 1, markerRect.width, 2), c);
                EditorGUI.DrawRect(markerRect, c);
                if (i == model.SelectedIndex)
                {
                    Handles.BeginGUI();
                    Handles.color = Color.white;
                    Handles.DrawAAPolyLine(2f, new Vector3(markerRect.xMin - 2, markerRect.yMin - 2), new Vector3(markerRect.xMax + 2, markerRect.yMin - 2), new Vector3(markerRect.xMax + 2, markerRect.yMax + 2), new Vector3(markerRect.xMin - 2, markerRect.yMax + 2), new Vector3(markerRect.xMin - 2, markerRect.yMin - 2));
                    Handles.EndGUI();
                }

                // Interaction
                var evt = Event.current;
                if (evt.type == EventType.MouseDown && evt.button == 0 && markerRect.Contains(evt.mousePosition))
                {
                    model.SelectedIndex = i;
                    isDraggingMarker = true;
                    draggingIndex = i;
                    evt.Use();
                }
            }

            // Drag logic
            var e = Event.current;
            if (isDraggingMarker && draggingIndex >= 0 && draggingIndex < model.Meteors.Count)
            {
                if (e.type == EventType.MouseDrag)
                {
                    float localX = Mathf.Clamp(e.mousePosition.x, rect.x, rect.xMax) - rect.x;
                    float t = model.TimelineScroll + localX * secondsPerPixel;
                    t = Mathf.Clamp(t, 0f, model.Duration);
                    var m = model.Meteors[draggingIndex];
                    if (Math.Abs(m.spawnTime - t) > 0.0001f)
                    {
                        m.spawnTime = t;
                    }
                    e.Use();
                }
                else if (e.type == EventType.MouseUp)
                {
                    isDraggingMarker = false;
                    draggingIndex = -1;
                    e.Use();
                }
            }

            // Background click to add/select
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                // Add new based on tool if active
                if (LevelEditorUtils.IsCreateTool(model.CurrentTool))
                {
                    float localX = Mathf.Clamp(Event.current.mousePosition.x, rect.x, rect.xMax) - rect.x;
                    float t = model.TimelineScroll + localX * secondsPerPixel;
                    t = Mathf.Clamp(t, 0f, model.Duration);
                    var data = LevelEditorUtils.CreateMeteor(Vector3.zero, model.CurrentTool, t);
                    model.Meteors.Add(data);
                    model.SelectedIndex = model.Meteors.Count - 1;
                    Event.current.Use();
                }
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
    }
}
