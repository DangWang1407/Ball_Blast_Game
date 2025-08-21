using UnityEngine;
using Game.Controllers;
using Game.Utils;

namespace Game.PowerUps.Missile
{
    // Homing Effect - Thay thế MissileHoming component
    public class HomingEffectComponent : PowerUpEffectComponent
    {
        [SerializeField] private float homingRate = 0.5f;
        [SerializeField] private float rotationSpeed = 5f;
        
        private Transform targetMeteor;
        private float lastTime;
        private MissileController missileController;
        private MissileStats missileStats;

        protected override void OnActivate()
        {
            autoDestroy = false; // Missile controls its own lifetime
            missileController = GetComponent<MissileController>();
            missileStats = GetComponent<MissileStats>();
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