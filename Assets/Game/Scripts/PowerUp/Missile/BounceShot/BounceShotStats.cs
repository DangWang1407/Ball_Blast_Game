using UnityEngine;

namespace Game.PowerUp
{
    public class BounceShotStats : PowerUpStats
    {
        [SerializeField] private int baseMaxBounces = 3;
        [SerializeField] private float levelScale = 1f;

        override public float GetDuration(int currentLevel)
        {
            return baseDuration + (currentLevel - 1) * levelScale * 5f;
        }

        public int GetMaxBounces(int currentLevel)
        {
            return baseMaxBounces + (currentLevel - 1);
        }
    }
}