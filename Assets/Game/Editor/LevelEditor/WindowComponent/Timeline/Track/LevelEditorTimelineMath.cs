using UnityEngine;

namespace Game.Editor
{
    public static class LevelEditorTimelineMath
    {
        // Converts a time value to a pixel position on the timeline
        public static float TimeToPixel(float t, Rect rect, LevelEditorModel model, float secondsPerPixel)
        {
            return rect.x + (t - model.TimelineScroll) / secondsPerPixel;
        }

        // Counts how many meteors are within a certain time range of t
        public static float CountAroundTime(LevelEditorModel model, float t, float range)
        {
            float count = 0f;
            for (int i = 0; i < model.Meteors.Count; i++)
            {
                if (Mathf.Abs(model.Meteors[i].spawnTime - t) <= range)
                {
                    if (model.Meteors[i].size == MeteorSize.Large) count += 2f;
                    else if (model.Meteors[i].size == MeteorSize.Medium) count += 1.5f;
                    else
                    count += 1f;
                }
            }
            return count;
        }
    }
}

