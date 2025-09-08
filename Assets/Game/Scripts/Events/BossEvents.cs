using UnityEngine;

namespace Game.Events
{
    public struct BossDeathEvent : IGameEvent
    {
        public Vector3 Position { get; }
        public BossDeathEvent(Vector3 position)
        {
            Position = position;
        }
    }
}