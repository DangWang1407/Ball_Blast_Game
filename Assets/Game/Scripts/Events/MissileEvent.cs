using UnityEngine;

namespace Game.Events
{
    public enum MissileDestroyReason
    {
        HitTarget,
        OutOfBounds
    }

    public struct MissileFiredEvent : IGameEvent
    {
        public Vector3 Position { get; }
        public float Speed { get; } 
    
        public MissileFiredEvent(Vector3 position, float speed)
        {
            Position = position;
            Speed = speed;
        }
    }

    public enum MissileType
    {
        Standard,
        Homing,
        Explosive
    }
}