using UnityEngine;

namespace Game.PowerUp
{
    public class DoubleShotStats : MonoBehaviour
    {
        [SerializeField] private float baseDuration = 20f;
        [SerializeField] private int baseBulletCountMultiplier = 2;
        [SerializeField] private float levelScale = 1f;

        public float GetDuration(int currentLevel)
        {
            return baseDuration + (currentLevel - 1) * levelScale * 4f;
        }

        public int GetBulletCountMultiplier(int currentLevel)
        {
            return baseBulletCountMultiplier + (currentLevel - 1);
        }
    }
}