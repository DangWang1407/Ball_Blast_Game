using Game.Controllers;
using Game.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PowerUp
{
    public class Homing : PowerUpEffect
    {
        [SerializeField] private float homingRate = 0.5f;
        [SerializeField] private float rotationSpeed = 5f;

        private Transform targetMeteor;
        private float lastTime;
        private MissileController missileController;
        private MissileStats missileStats;

        protected override void OnActivate()
        {
            missileController = GetComponent<MissileController>();
            missileStats = GetComponent<MissileStats>();
        }

        protected override void OnDeactivate()
        {
            
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