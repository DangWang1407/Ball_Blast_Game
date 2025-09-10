using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class LevelEditorTimelineGUI
    {
        private const float HeaderHeight = 22f;
        private const float RulerHeight = 24f;
        private const float TrackHeight = 250f;
        private readonly LevelEditorTimelineHeaderGUI headerGUI = new LevelEditorTimelineHeaderGUI();
        private readonly LevelEditorTimelineRulerGUI rulerGUI = new LevelEditorTimelineRulerGUI();
        private readonly LevelEditorTimelineTrackGUI trackGUI = new LevelEditorTimelineTrackGUI();


        public void Draw(LevelEditorModel model, Rect rect)
        {
            if (Event.current.type == EventType.Repaint)
            {
                EditorGUI.DrawRect(rect, new Color(0.15f, 0.15f, 0.15f));
            }

            var header = new Rect(rect.x, rect.y, rect.width, HeaderHeight);
            headerGUI.Draw(model, header);

            var ruler = new Rect(rect.x, rect.y + HeaderHeight, rect.width, RulerHeight);
            rulerGUI.Draw(model, ruler);

            float available = Mathf.Max(32f, rect.height - HeaderHeight - RulerHeight - 8);
            var track = new Rect(rect.x + 8, ruler.yMax + (available - TrackHeight) * 0.5f, rect.width - 16, Mathf.Min(TrackHeight, available));
            trackGUI.Draw(model, track);
            return;
        }
    }
}
