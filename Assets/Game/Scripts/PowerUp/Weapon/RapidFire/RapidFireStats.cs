using UnityEngine;

namespace Game.PowerUp
{
    public class RapidFireStats : MonoBehaviour
    {
        [SerializeField] private float baseDuration = 20f;
        [SerializeField] private float baseFireRateMultiplier = 4f;
        [SerializeField] private float levelScale = 1f;

        public float GetDuration(int currentLevel)
        {
            return baseDuration + (currentLevel - 1) * levelScale * 5f;
        }

        public float GetFireRateMultiplier(int currentLevel)
        {
            return baseFireRateMultiplier + (currentLevel - 1) * levelScale * 0.1f;
        }
    }
}