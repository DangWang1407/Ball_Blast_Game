using UnityEngine;
using Game.Controllers;

namespace Game.PowerUpV2
{
    public class DoubleShotBehavior : MonoBehaviour
    {
        private WeaponStats weaponStats;
        private int originalBulletCount;
        private int multiplier = 2;

        public void Init(int bulletCountMultiplier)
        {
            multiplier = Mathf.Max(1, bulletCountMultiplier);
            Apply();
        }

        private void OnEnable()
        {
            weaponStats = GetComponent<WeaponStats>();
            if (weaponStats != null)
            {
                originalBulletCount = weaponStats.BulletCount;
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
            if (originalBulletCount <= 0) originalBulletCount = weaponStats.BulletCount;
            weaponStats.BulletCount = originalBulletCount * multiplier;
        }

        private void Restore()
        {
            if (weaponStats == null) return;
            weaponStats.BulletCount = originalBulletCount;
        }
    }
}

