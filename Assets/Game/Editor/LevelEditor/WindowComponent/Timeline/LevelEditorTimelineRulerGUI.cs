using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class LevelEditorTimelineRulerGUI
    {
        public void Draw(LevelEditorModel model, Rect rect)
        {
            if (Event.current.type == EventType.Repaint)
            {
                EditorGUI.DrawRect(rect, new Color(0.16f, 0.16f, 0.16f));
            }

            float total = Mathf.Max(1f, model.Duration);
            float secondsPerPixel = total / rect.width / model.TimelineZoom;
            float visibleSpan = secondsPerPixel * rect.width;
            model.TimelineScroll = Mathf.Clamp(model.TimelineScroll, 0f, Mathf.Max(0f, total - visibleSpan));

            Handles.BeginGUI();
            Handles.color = new Color(1f, 1f, 1f, 0.2f);
            Handles.DrawLine(new Vector3(rect.x, rect.yMax, 0), new Vector3(rect.xMax, rect.yMax, 0));

            float[] candidates = { 0.1f, 0.25f, 0.5f, 1f, 2f, 5f, 10f };
            float minPx = 60f;
            float step = 1f;
            foreach (var c in candidates)
            {
                if (c / secondsPerPixel >= minPx) { step = c; break; }
            }

            // Draw ticks and labels
            GUIStyle tickStyle = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.UpperCenter };
            for (float t = Mathf.Floor(model.TimelineScroll / step) * step; t <= model.TimelineScroll + visibleSpan + 0.001f; t += step)
            {
                float x = TimeToPixel(t, rect, model, secondsPerPixel);
                Handles.color = new Color(1f, 1f, 1f, 0.25f);
                Handles.DrawLine(new Vector3(x, rect.y + 6, 0), new Vector3(x, rect.yMax, 0));
                GUI.Label(new Rect(x - 30, rect.y + 2, 60, 16), t.ToString("0.##s"), tickStyle);
            }
            Handles.EndGUI();

            // Scrollbar - use a fixed-size scrollbar for viewport width
            float totalPixels = total / secondsPerPixel;                 // total length in pixels at current zoom
            float viewSizePixels = visibleSpan / secondsPerPixel;        // viewport size in pixels 
            float scrollPixels = model.TimelineScroll / secondsPerPixel; // current scroll in pixels
            var scrollRect = new Rect(rect.x + 6, rect.yMax - 8, rect.width - 12, 6);
            float newScrollPixels = GUI.HorizontalScrollbar(scrollRect, scrollPixels, viewSizePixels, 0f, totalPixels);
            if (!Mathf.Approximately(newScrollPixels, scrollPixels))
            {
                model.TimelineScroll = Mathf.Clamp(newScrollPixels * secondsPerPixel, 0f, Mathf.Max(0f, total - visibleSpan));
            }
        }

        private static float TimeToPixel(float t, Rect rect, LevelEditorModel model, float secondsPerPixel)
        {
            return rect.x + (t - model.TimelineScroll) / secondsPerPixel;
        }
    }
}
