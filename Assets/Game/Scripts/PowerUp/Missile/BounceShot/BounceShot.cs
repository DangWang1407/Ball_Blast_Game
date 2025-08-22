using Game.Controllers;
using UnityEngine;

namespace Game.PowerUp
{
    public class BounceShot : PowerUpEffect
    {
        private bool originalCanBounce;
        private MissileStats missileStats;
        private BounceShotStats bounceShotStats;

        protected override void OnActivate()
        {
            if (gameObject.CompareTag("Missile"))
            {
                missileStats = GetComponent<MissileStats>();
                originalCanBounce = missileStats.CanBounce;

                missileStats.CanBounce = true;
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (collision.CompareTag("Player"))
            {
                powerUpType = PowerUpType.BounceShot;

                if (gameObject.CompareTag("PowerUp"))
                {
                    currentLevel = levelPowerUpManager.GetLevel(PowerUpType.BounceShot);
                    bounceShotStats = GetComponent<BounceShotStats>();

                    timer = bounceShotStats.GetDuration(currentLevel);
                }
            }
        }

        protected override void OnDeactivate()
        {
            if (gameObject.CompareTag("Missile"))
            {
                missileStats.CanBounce = originalCanBounce;
            }
        }
    }
}