using UnityEngine;

namespace Game.PowerUp
{
    public class PowerShotStats : MonoBehaviour
    {
        [SerializeField] private float baseDuration = 20f;
        [SerializeField] private float baseScaleMultiplier = 1.5f;
        [SerializeField] private float levelScale = 1f;

        public float GetDuration(int currentLevel)
        {
            return baseDuration + (currentLevel - 1) * levelScale * 4f;
        }

        public float GetScaleMultiplier(int currentLevel)
        {
            return baseScaleMultiplier + (currentLevel - 1) * levelScale * 0.2f;
        }
    }
}