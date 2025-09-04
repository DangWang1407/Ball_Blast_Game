using Game.Controllers;
using UnityEngine;

namespace Game.PowerUp
{
    public class PierceShot : PowerUpEffect, IMissileCollisionBehavior
    {
        private PierceShotStats pierceShotStats;
        private MissileCollision missileCollision;

        protected override void OnActivate()
        {
            if (gameObject.CompareTag("Missile"))
            {
                missileCollision = GetComponent<MissileCollision>();
                if (missileCollision != null)
                {
                    missileCollision.AddBehavior(this);
                }
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
            if (missileCollision != null)
            {
                missileCollision.RemoveBehavior(this);
            }
        }

        public bool HandleWallCollision(Collider2D wall, MissileMovement movement)
        {
            return false;
        }

        public bool HandleGroundCollision(Collider2D ground, MissileMovement movement)
        {
            return false;
        }

        public bool HandleMeteorCollision(Collider2D meteor)
        {
            return true;
        }
    }
}

