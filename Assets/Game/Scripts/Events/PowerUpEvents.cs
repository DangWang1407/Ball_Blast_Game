using UnityEngine;

namespace Game.Events
{
    public struct  PowerUpSpawnEvent : IGameEvent
    {
        public Vector3 Position { get; }
        public MeteorSize MeteorSize { get; }

        public PowerUpSpawnEvent(Vector3 position, MeteorSize meteorSize)
        {
            Position = position;
            MeteorSize = meteorSize;
        }
    }

    //public struct WeaponPowerUpEvent : IGameEvent
    //{
    //    public PowerUpType PowerUpType { get; }
    //    public float Duration { get; }
    //    public WeaponPowerUpEvent(PowerUpType powerUpType, float duration)
    //    {
    //        PowerUpType = powerUpType;
    //        Duration = duration;
    //    }
    //}

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

    public enum PowerUpType
    {
        RapidFire,
        DoubleShot,
        PowerShot,
        PierceShot,
        BurstShot,
        DamageBoost, 
        Invisible,
        Shield, 
        Homing
    }
}