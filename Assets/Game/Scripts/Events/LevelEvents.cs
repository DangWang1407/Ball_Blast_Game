using UnityEngine;

namespace Game.Events
{
    public struct LevelStartEvent : IGameEvent
    {
        public int LevelIndex { get; }
        public TextAsset LevelData { get; }
        public LevelStartEvent(int levelIndex, TextAsset levelData)
        {
            LevelIndex = levelIndex;
            LevelData = levelData;
        }
    }

    public struct AllMeteorsDestroyedEvent : IGameEvent
    {
        public int LevelIndex { get; }
        public AllMeteorsDestroyedEvent(int levelIndex)
        {
            LevelIndex = levelIndex;
        }
    }

    //public struct GameCompleteEvent : IGameEvent
    //{
    //    public GameCompleteEvent() { }
    //}
}
