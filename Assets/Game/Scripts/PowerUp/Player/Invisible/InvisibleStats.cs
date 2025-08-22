using UnityEngine;

namespace Game.PowerUp
{
    public class InvisibleStats : MonoBehaviour
    {
        [SerializeField] private float baseDuration = 20f;
        [SerializeField] private float alphaValue = 0.5f;
        [SerializeField] private float levelScale = 1.0f;

        public float GetDuration(int currentLevel)
        {
            return baseDuration + (currentLevel - 1) * levelScale;
        }

        public float GetAlphaValue(int currentLevel)
        {
            return alphaValue - (currentLevel - 1) * levelScale * 0.1f;
        }
    }
}