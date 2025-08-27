using UnityEngine;
using Game.PowerUp;

namespace Game.Events
{
    public struct PowerUpSpawnEvent : IGameEvent
    {
        public Vector3 Position { get; }
        public MeteorSize MeteorSize { get; }

        public PowerUpSpawnEvent(Vector3 position, MeteorSize meteorSize)
        {
            Position = position;
            MeteorSize = meteorSize;
        }
    }

    public struct SpecificPowerUpSpawnEvent : IGameEvent
    {
        public Vector3 Position { get; }
        public PowerUpType PowerUpType { get; }
        public SpecificPowerUpSpawnEvent(Vector3 position, PowerUpType type)
        {
            Position = position;
            PowerUpType = type;
        }
    }

    public struct PowerUpCollectedEvent : IGameEvent
    {
        public PowerUpType PowerUpType;
        public float Duration;
        public PowerUpCollectedEvent(PowerUpType powerUpType, float duration)
        {
            PowerUpType = powerUpType;
            Duration = duration;
        }
    }

    //public enum PowerUpType
    //{
    //    RapidFire,
    //    DoubleShot,
    //    PowerShot,
    //    PierceShot,
    //    BurstShot,
    //    DamageBoost, 
    //    Invisible,
    //    Shield, 
    //    Homing,
    //    DiagonalFire,
    //    BounceShot
    //}
}