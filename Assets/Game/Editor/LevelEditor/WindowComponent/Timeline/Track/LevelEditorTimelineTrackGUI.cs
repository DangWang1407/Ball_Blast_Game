using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class LevelEditorTimelineTrackGUI
    {
        private const float MarkerRadius = 6f;
        private const float StackThresholdSeconds = 0.3f;
        private const float StackSpacingPixels = 20f;
        private const float RailYOffset = 50f;

        private bool isDraggingMarker = false;
        private int draggingIndex = -1;

        private bool isSelectingRect = false;
        private Vector2 selectStart;
        private Vector2 selectEnd;

        private readonly LevelEditorTimelineDensity density = new();
        private readonly LevelEditorTimelineMarkers markers = new();
        private readonly LevelEditorTimelineDrag drag = new();
        private readonly LevelEditorTimelineSelection selection = new();
        private readonly LevelEditorTimelineStacking stacking = new();

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
            density.Draw(rect, model, secondsPerPixel);

            var stackOffsets = stacking.Compute(model, StackThresholdSeconds, StackSpacingPixels);
            markers.Draw(rect, model, secondsPerPixel, railY, stackOffsets, MarkerRadius, ref isDraggingMarker, ref draggingIndex);

            drag.Handle(rect, model, secondsPerPixel, total, ref isDraggingMarker, ref draggingIndex);
            selection.DrawOverlay(ref isSelectingRect, ref selectStart, ref selectEnd);
            selection.Finalize(rect, model, secondsPerPixel, railY, stackOffsets, MarkerRadius, ref isSelectingRect, selectStart, selectEnd);
            HandleBackgroundMouseDown(rect, model, secondsPerPixel, total);
        }

        private static void DrawBaseline(Rect rect, float railY)
        {
            Handles.BeginGUI();
            Handles.color = new Color(1f, 1f, 1f, 0.2f);
            Handles.DrawLine(new Vector3(rect.x, railY, 0), new Vector3(rect.xMax, railY, 0));
            Handles.EndGUI();
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

        
    }
}
