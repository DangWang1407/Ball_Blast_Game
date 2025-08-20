using UnityEngine;

namespace Game.Events
{
    public struct MeteorDestroyedEvent : IGameEvent
    {
        public MeteorSize Size { get; }
        public Vector3 Position { get; }
        public MeteorDestroyedEvent(MeteorSize size, Vector3 position)
        {
            Size = size;
            Position = position;
        }
    }

    public struct SplitMeteorEvent : IGameEvent
    {
        public Vector3 Position { get; }
        public MeteorSize MeteorSize { get; }
        public SplitMeteorEvent(Vector3 position, MeteorSize meteorSize)
        {
            Position = position;
            MeteorSize = meteorSize;
        }
    }
}