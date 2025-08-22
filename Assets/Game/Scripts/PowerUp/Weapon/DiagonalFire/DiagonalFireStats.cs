using UnityEngine;

namespace Game.PowerUp
{
    public class DiagonalFireStats : MonoBehaviour
    {
        [SerializeField] private float baseDuration = 20f;
        [SerializeField] private int baseExtraMissileCount = 2;
        [SerializeField] private float baseFireRateMultiplier = 1.2f;
        [SerializeField] private float levelScale = 1f;

        public float GetDuration(int currentLevel)
        {
            return baseDuration + (currentLevel - 1) * levelScale * 4f;
        }

        public int GetExtraMissileCount(int currentLevel)
        {
            return baseExtraMissileCount + (currentLevel - 1);
        }

        public float GetFireRateMultiplier(int currentLevel)
        {
            return baseFireRateMultiplier + (currentLevel - 1) * levelScale * 0.1f;
        }
    }
}