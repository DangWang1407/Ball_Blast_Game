using Game.Controllers;
using Game.Utils;
using UnityEngine;

namespace Game.PowerUp
{
    public class Homing : PowerUpEffect
    {
        private float homingRate = 0.5f;
        private float rotationSpeed = 5f;

        private Transform targetMeteor;
        private float lastTime;
        private MissileController missileController;
        private MissileStats missileStats;
        private HomingStats homingStats;

        protected override void OnActivate()
        {
            if(gameObject.CompareTag("Missile"))
            {
                missileController = GetComponent<MissileController>();
                missileStats = GetComponent<MissileStats>();
            }
        }

        protected override void OnDeactivate()
        {
            
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (collision.CompareTag("Player"))
            {
                powerUpType = PowerUpType.Homing;

                if (gameObject.CompareTag("PowerUp"))
                {
                    currentLevel = levelPowerUpManager.GetLevel(PowerUpType.Homing);
                    homingStats = GetComponent<HomingStats>();

                    timer = homingStats.GetDuration(currentLevel);
                    homingRate = homingStats.GetHomingRate(currentLevel);
                    rotationSpeed = homingStats.GetRotationSpeed(currentLevel);
                }
            }
        }

        protected override void OnUpdate()
        {
            if (missileController == null) return;

            if (Time.time - lastTime > homingRate)
            {
                lastTime = Time.time;
                FindClosestMeteor();
            }

            if (targetMeteor != null)
            {
                NavigateToTarget();
            }
        }

        private void FindClosestMeteor()
        {
            float closestDistance = float.MaxValue;
            targetMeteor = null;

            foreach (var meteor in ChaseableEntity.AllEntities)
            {
                float distance = Vector2.Distance(transform.position, meteor.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetMeteor = meteor.transform;
                }
            }
        }

        private void NavigateToTarget()
        {
            Vector2 direction = (targetMeteor.position - transform.position).normalized;
            missileController.Rigidbody.velocity = direction * missileStats.MissileSpeed;

            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}