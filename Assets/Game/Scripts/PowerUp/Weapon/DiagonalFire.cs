using Game.Controllers;
using UnityEngine;

namespace Game.PowerUp
{
    public class DiagonalFire : PowerUpEffect
    {
        private WeaponShooting weaponShooting;
        private WeaponStats weaponStats;
        private float lastFireTime = 0;

        protected override void OnActivate()
        {
            weaponShooting = GetComponent<WeaponShooting>();
            weaponStats = GetComponent<WeaponStats>();

            if (weaponShooting == null || weaponStats == null)
            {
                Debug.LogError("Required components not found!");
                return;
            }
        }

        private void FixedUpdate()
        {
            if (weaponShooting != null && Time.time - lastFireTime >= weaponStats.FireRate)
            {
                lastFireTime = Time.time;
                weaponShooting.FireNormalMissile(new Vector2(1, 1));
                weaponShooting.FireNormalMissile(new Vector2(-1, 1));
            }
        }

        protected override void OnDeactivate()
        {
            // Logic dọn dẹp khi power-up hết hiệu lực
        }
    }
}