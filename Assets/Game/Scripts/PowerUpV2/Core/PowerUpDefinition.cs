using UnityEngine;
using Game.PowerUp;

namespace Game.PowerUpV2
{
    public abstract class PowerUpDefinition : ScriptableObject
    {
        [SerializeField] private PowerUpType powerUpType;
        [SerializeField] private float baseDuration = 20f;
        [SerializeField] private float durationPerLevel = 5f;

        public PowerUpType Type => powerUpType;

        public virtual float GetDuration(int level)
        {
            if (level < 1) level = 1;
            return baseDuration + (level - 1) * durationPerLevel;
        }
    }
}

