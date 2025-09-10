using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class LevelEditorTimelineDensity
    {
        private const float DensityHeight = 200f;
        private const float DensityBottomMargin = 4f;

        public void Draw(Rect rect, LevelEditorModel model, float secondsPerPixel)
        {
            var densityRect = new Rect(rect.x, rect.yMax - DensityHeight - DensityBottomMargin, rect.width, DensityHeight);
            Handles.BeginGUI();
            Vector3? prev = null;
            for (int x = 0; x < densityRect.width; x += 4)
            {
                float t = model.TimelineScroll + x * secondsPerPixel;
                float count = LevelEditorTimelineMath.CountAroundTime(model, t, 1f);
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
    }
}
