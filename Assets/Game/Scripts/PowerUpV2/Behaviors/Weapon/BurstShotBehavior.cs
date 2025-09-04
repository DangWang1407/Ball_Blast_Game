using UnityEngine;
using Game.Controllers;

namespace Game.PowerUpV2
{
    public class BurstShotBehavior : MonoBehaviour
    {
        private WeaponStats weaponStats;
        private int originalBurstCount;
        private float originalBurstDelay;
        private int burstCount = 2;
        private float burstDelay = 0.05f;

        public void Init(int count, float delay)
        {
            burstCount = Mathf.Max(1, count);
            burstDelay = Mathf.Max(0f, delay);
            Apply();
        }

        private void OnEnable()
        {
            weaponStats = GetComponent<WeaponStats>();
            if (weaponStats != null)
            {
                originalBurstCount = weaponStats.BurstCount;
                originalBurstDelay = weaponStats.BurstDelay;
            }
            Apply();
        }

        private void OnDisable()
        {
            Restore();
        }

        private void Apply()
        {
            if (weaponStats == null) weaponStats = GetComponent<WeaponStats>();
            if (weaponStats == null) return;
            weaponStats.BurstCount = burstCount;
            weaponStats.BurstDelay = burstDelay;
        }

        private void Restore()
        {
            if (weaponStats == null) return;
            weaponStats.BurstCount = originalBurstCount;
            weaponStats.BurstDelay = originalBurstDelay;
        }
    }
}

