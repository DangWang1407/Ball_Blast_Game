using Game.Controllers;
using UnityEngine;

namespace Game.PowerUp
{
    public class BounceShot : PowerUpEffect, IMissileCollisionBehavior
    {
        private BounceShotStats bounceShotStats;
        private MissileCollision missileCollision;

        private int maxBounces = 3;
        private int bounceLeft = 0;
        private bool isEffectActive = false; 

        protected override void OnActivate()
        {
            powerUpType = PowerUpType.BounceShot;
            levelPowerUpManager = GameObject.FindWithTag("Player").GetComponent<LevelPowerUp>();
           
            currentLevel = levelPowerUpManager.GetLevel(PowerUpType.BounceShot);
            bounceShotStats = GetComponent<BounceShotStats>();
            if (bounceShotStats != null)
            {
                timer = bounceShotStats.GetDuration(currentLevel);
            }
            
            missileCollision = GetComponent<MissileCollision>();
            if (missileCollision != null)
            {
                missileCollision.AddBehavior(this);
                isEffectActive = true;

                ResetBounceCount();
            }            
        }

        public void ResetBounceCount()
        {
            bounceLeft = GetBounceCount(currentLevel);
        }

        void OnEnable()
        {
            if (isEffectActive && gameObject.CompareTag("Missile"))
            {
                ResetBounceCount();
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            // base.OnTriggerEnter2D(collision);
            // if (collision.CompareTag("Player"))
            // {
            //     powerUpType = PowerUpType.BounceShot;

                // if (gameObject.CompareTag("PowerUp"))
                // {
                    // currentLevel = levelPowerUpManager.GetLevel(PowerUpType.BounceShot);
                    // bounceShotStats = GetComponent<BounceShotStats>();

                    // timer = bounceShotStats.GetDuration(currentLevel);
                // }
            // }
        }

        protected override void OnDeactivate()
        {
            isEffectActive = false;
            if (missileCollision != null)
            {
                missileCollision.RemoveBehavior(this);
            }
        }

        public bool HandleWallCollision(Collider2D wall, MissileMovement movement)
        {
            if (bounceLeft > 0)
            {
                bounceLeft--;
                movement.Reflect(wall.transform.right);

                return true; 
            }
            return false;
        }

        public bool HandleGroundCollision(Collider2D ground, MissileMovement movement)
        {
            if (bounceLeft > 0)
            {
                bounceLeft--;
                movement.Reflect(ground.transform.up);

                return true;
            }
            return false;
        }

        public bool HandleMeteorCollision(Collider2D meteor)
        {
            return false;
        }

        private int GetBounceCount(int level)
        {
            if (bounceShotStats != null)
                return bounceShotStats.GetMaxBounces(level);
            return maxBounces;
        }
    }
}