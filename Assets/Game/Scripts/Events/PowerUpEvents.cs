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

    public struct PowerUpV2ActivatedEvent : IGameEvent
    {
        public Game.PowerUp.PowerUpType PowerUpType { get; }
        public Game.PowerUpV2.PowerUpDefinition Definition { get; }
        public int Level { get; }

        public PowerUpV2ActivatedEvent(Game.PowerUp.PowerUpType type, Game.PowerUpV2.PowerUpDefinition definition, int level)
        {
            PowerUpType = type;
            Definition = definition;
            Level = level;
        }
    }

    public struct PowerUpV2ExpiredEvent : IGameEvent
    {
        public Game.PowerUp.PowerUpType PowerUpType { get; }
        public Game.PowerUpV2.PowerUpDefinition Definition { get; }
        public int Level { get; }

        public PowerUpV2ExpiredEvent(Game.PowerUp.PowerUpType type, Game.PowerUpV2.PowerUpDefinition definition, int level)
        {
            PowerUpType = type;
            Definition = definition;
            Level = level;
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
