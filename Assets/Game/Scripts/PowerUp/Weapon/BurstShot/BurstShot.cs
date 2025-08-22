using Game.Controllers;
using UnityEngine;

namespace Game.PowerUp
{
    public class BurstShot : PowerUpEffect
    {
        private int burstCount = 2;
        private float burstDelay = 0.05f;

        private int originalBurstCount;
        private float originalBurstDelay;
        private WeaponStats weaponStats;
        private BurstShotStats burstShotStats;

        protected override void OnActivate()
        {
            if (gameObject.CompareTag("Weapon"))
            {
                weaponStats = GetComponent<WeaponStats>();
                originalBurstCount = weaponStats.BurstCount;
                originalBurstDelay = weaponStats.BurstDelay;

                weaponStats.BurstCount = burstCount;
                weaponStats.BurstDelay = burstDelay;
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (collision.CompareTag("Player"))
            {
                powerUpType = PowerUpType.BurstShot;

                if (gameObject.CompareTag("PowerUp"))
                {
                    currentLevel = levelPowerUpManager.GetLevel(PowerUpType.BurstShot);
                    burstShotStats = GetComponent<BurstShotStats>();

                    timer = burstShotStats.GetDuration(currentLevel);
                    burstCount = burstShotStats.GetBurstCount(currentLevel);
                    burstDelay = burstShotStats.GetBurstDelay(currentLevel);
                }
            }
        }

        protected override void OnDeactivate()
        {
            if (gameObject.CompareTag("Weapon"))
            {
                weaponStats.BurstCount = originalBurstCount;
                weaponStats.BurstDelay = originalBurstDelay;
            }
        }
    }
}