using System.Collections.Generic;

namespace Game.Editor
{
    public class LevelEditorTimelineStacking
    {
        public Dictionary<int, float> Compute(LevelEditorModel model, float threshold, float spacing)
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
