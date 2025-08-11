using UnityEngine;

namespace Game.Events
{
    public struct PlayerDeathEvent : IGameEvent
    {
        public int RemainingLives { get; }
        public DeathCause Cause { get; }
        public PlayerDeathEvent(int remainingLives, DeathCause cause)
        {
            RemainingLives = remainingLives;
            Cause = cause;
        }

    }

    public enum DeathCause
    {
        MeteorHit,
        Other
    }
}