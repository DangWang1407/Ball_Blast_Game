using Game.Controllers;
using UnityEngine;

namespace Game.Editor
{
    public static class LevelEditorUtils
    {
        public static bool IsCreateTool(ToolMode tool)
            => tool == ToolMode.CreateLarge || tool == ToolMode.CreateMedium || tool == ToolMode.CreateSmall;

        public static MeteorSize ToMeteorSize(ToolMode tool)
            => tool switch
            {
                ToolMode.CreateLarge => MeteorSize.Large,
                ToolMode.CreateMedium => MeteorSize.Medium,
                ToolMode.CreateSmall => MeteorSize.Small,
                _ => MeteorSize.Small
            };

        public static int DefaultHealth(MeteorSize size)
            => size switch
            {
                MeteorSize.Large => 10,
                MeteorSize.Medium => 5,
                _ => 2
            };

        public static MeteorData CreateMeteor(Vector3 position, ToolMode tool, float spawnTime)
        {
            var size = ToMeteorSize(tool);
            return new MeteorData
            {
                position = position,
                size = size,
                health = DefaultHealth(size),
                spawnTime = Mathf.Max(0f, spawnTime)
            };
        }

        public static void ClampSpawnTimes(LevelEditorModel model)
        {
            float max = Mathf.Max(0.0001f, model.Duration);
            for (int i = 0; i < model.Meteors.Count; i++)
            {
                var m = model.Meteors[i];
                if (m.spawnTime > max) m.spawnTime = max;
                if (m.spawnTime < 0f) m.spawnTime = 0f;
            }
        }
    }
}
