using UnityEngine;

namespace Game.PowerUp
{
    public class BurstShotStats : MonoBehaviour
    {
        [SerializeField] private float baseDuration = 20f;
        [SerializeField] private int baseBurstCount = 2;
        [SerializeField] private float baseBurstDelay = 0.05f;
        [SerializeField] private float levelScale = 1.0f;

        public float GetDuration(int currentLevel)
        {
            return baseDuration + (currentLevel - 1) * levelScale * 5f;
        }

        public int GetBurstCount(int currentLevel)
        {
            return baseBurstCount + (currentLevel - 1);
        }

        public float GetBurstDelay(int currentLevel)
        {
            return baseBurstDelay - (currentLevel - 1) * levelScale * 0.01f;
        }
    }
}