using Game.Controllers;
using UnityEngine;

namespace Game.PowerUp
{
    public class PierceShot : PowerUpEffect
    {
        private bool originalCanPierce;
        private MissileStats missileStats;
        private PierceShotStats pierceShotStats;

        protected override void OnActivate()
        {
            if (gameObject.CompareTag("Missile"))
            {
                missileStats = GetComponent<MissileStats>();
                originalCanPierce = missileStats.CanPierce;

                missileStats.CanPierce = true;
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (collision.CompareTag("Player"))
            {
                powerUpType = PowerUpType.PierceShot;

                if (gameObject.CompareTag("PowerUp"))
                {
                    currentLevel = levelPowerUpManager.GetLevel(PowerUpType.PierceShot);
                    pierceShotStats = GetComponent<PierceShotStats>();

                    timer = pierceShotStats.GetDuration(currentLevel);
                }
            }
        }

        protected override void OnDeactivate()
        {
            if (gameObject.CompareTag("Missile"))
            {
                missileStats.CanPierce = originalCanPierce;
            }
        }
    }
}