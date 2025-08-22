using UnityEngine;

namespace Game.PowerUp
{
    public class HomingStats : MonoBehaviour
    {
        [SerializeField] private float baseDuration = 20f;
        [SerializeField] private float baseHomingRate = 0.5f;
        [SerializeField] private float baseRotationSpeed = 5f;
        [SerializeField] private float levelScale = 1f;

        public float GetDuration(int currentLevel)
        {
            return baseDuration + (currentLevel - 1) * levelScale * 5f;
        }

        public float GetHomingRate(int currentLevel)
        {
            return Mathf.Max(0.1f, baseHomingRate - (currentLevel - 1) * levelScale * 0.05f);
        }

        public float GetRotationSpeed(int currentLevel)
        {
            return baseRotationSpeed + (currentLevel - 1) * levelScale * 1f;
        }
    }
}