using UnityEngine;
using Game.Controllers;

namespace Game.PowerUpV2
{
    public class PowerShotBehavior : MonoBehaviour
    {
        private MissileStats missileStats;
        private float originalScale;
        private float multiplier = 1.0f;

        public void Init(float scaleMultiplier)
        {
            multiplier = scaleMultiplier;
            Apply();
        }

        private void OnEnable()
        {
            missileStats = GetComponent<MissileStats>();
            if (missileStats != null)
            {
                originalScale = missileStats.BulletScale;
            }
            Apply();
        }

        private void OnDisable()
        {
            Restore();
        }

        private void Apply()
        {
            if (missileStats == null) missileStats = GetComponent<MissileStats>();
            if (missileStats == null) return;
            if (originalScale <= 0f) originalScale = missileStats.BulletScale;
            missileStats.BulletScale = originalScale * multiplier;
        }

        private void Restore()
        {
            if (missileStats == null) return;
            missileStats.BulletScale = originalScale;
        }
    }
}

