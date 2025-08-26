using Game.Controllers;
using UnityEngine;

namespace Game.PowerUp
{
    public class DoubleShot : PowerUpEffect
    {
        private int bulletCountMultiplier = 2;
        private int originalBulletCount;
        private WeaponStats weaponStats;
        private DoubleShotStats doubleShotStats;

        protected override void OnActivate()
        {
            if (gameObject.CompareTag("Weapon"))
            {
                weaponStats = GetComponent<WeaponStats>();
                originalBulletCount = weaponStats.BulletCount;
                weaponStats.BulletCount = originalBulletCount * bulletCountMultiplier;
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (collision.CompareTag("Player"))
            {
                powerUpType = PowerUpType.DoubleShot;

                if (gameObject.CompareTag("PowerUp"))
                {
                    currentLevel = levelPowerUpManager.GetLevel(PowerUpType.DoubleShot);
                    doubleShotStats = GetComponent<DoubleShotStats>();

                    timer = doubleShotStats.GetDuration(currentLevel);
                    bulletCountMultiplier = doubleShotStats.GetBulletCountMultiplier(currentLevel);
                }
            }
        }

        protected override void OnDeactivate()
        {
            if (gameObject.CompareTag("Weapon"))
                weaponStats.BulletCount = originalBulletCount;
        }
    }
}