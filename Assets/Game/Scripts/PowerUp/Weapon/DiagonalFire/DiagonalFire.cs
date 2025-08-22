using Game.Controllers;
using UnityEngine;

namespace Game.PowerUp
{
    public class DiagonalFire : PowerUpEffect
    {
        private int extraMissileCount = 2;
        private float fireRateMultiplier = 1.2f;

        private WeaponShooting weaponShooting;
        private WeaponStats weaponStats;
        private float originalFireRate;
        private float lastFireTime = 0;
        private DiagonalFireStats diagonalFireStats;

        private float angleRange;

        protected override void OnActivate()
        {
            weaponShooting = GetComponent<WeaponShooting>();
            weaponStats = GetComponent<WeaponStats>();

            if (weaponStats != null)
            {
                originalFireRate = weaponStats.FireRate;
                weaponStats.FireRate = originalFireRate / fireRateMultiplier;
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (collision.CompareTag("Player"))
            {
                powerUpType = PowerUpType.DiagonalFire;

                if (gameObject.CompareTag("PowerUp"))
                {
                    currentLevel = levelPowerUpManager.GetLevel(PowerUpType.DiagonalFire);
                    diagonalFireStats = GetComponent<DiagonalFireStats>();

                    timer = diagonalFireStats.GetDuration(currentLevel);
                    extraMissileCount = diagonalFireStats.GetExtraMissileCount(currentLevel);
                    fireRateMultiplier = diagonalFireStats.GetFireRateMultiplier(currentLevel);

                    angleRange = 180f / (float)(extraMissileCount + 1);
                }
            }
        }

        private void FixedUpdate()
        {
            if (weaponShooting != null && Time.time - lastFireTime >= weaponStats.FireRate)
            {
                lastFireTime = Time.time;

                for (int i = 0; i < extraMissileCount; i++)
                {
                    float angle = 45f + (i * 90f); 
                    Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                    weaponShooting.FireNormalMissile(direction);
                }
            }
        }

        protected override void OnDeactivate()
        {
            if (weaponStats != null)
                weaponStats.FireRate = originalFireRate;
        }
    }
}