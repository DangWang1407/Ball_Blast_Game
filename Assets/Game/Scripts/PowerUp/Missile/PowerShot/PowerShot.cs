using Game.Controllers;
using UnityEngine;

namespace Game.PowerUp
{
    public class PowerShot : PowerUpEffect
    {
        private float scaleMultiplier = 1.5f;
        private float originalBulletScale;
        private MissileStats missileStats;
        private PowerShotStats powerShotStats;

        protected override void OnActivate()
        {
            if (gameObject.CompareTag("Missile"))
            {
                missileStats = GetComponent<MissileStats>();
                originalBulletScale = missileStats.BulletScale;
                missileStats.BulletScale = originalBulletScale * scaleMultiplier;
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (collision.CompareTag("Player"))
            {
                powerUpType = PowerUpType.PowerShot;

                if (gameObject.CompareTag("PowerUp"))
                {
                    currentLevel = levelPowerUpManager.GetLevel(PowerUpType.PowerShot);
                    powerShotStats = GetComponent<PowerShotStats>();

                    timer = powerShotStats.GetDuration(currentLevel);
                    scaleMultiplier = powerShotStats.GetScaleMultiplier(currentLevel);
                }
            }
        }

        protected override void OnDeactivate()
        {
            if (gameObject.CompareTag("Missile"))
                missileStats.BulletScale = originalBulletScale;
        }
    }
}