using UnityEngine;

namespace Game.Editor
{
    public static class LevelEditorTimelineMath
    {
        public static float TimeToPixel(float t, Rect rect, LevelEditorModel model, float secondsPerPixel)
        {
            return rect.x + (t - model.TimelineScroll) / secondsPerPixel;
        }

        public static float CountAroundTime(LevelEditorModel model, float t, float range)
        {
            float count = 0f;
            for (int i = 0; i < model.Meteors.Count; i++)
            {
                if (Mathf.Abs(model.Meteors[i].spawnTime - t) <= range) count += 1f;
            }
            return count;
        }
    }
}

