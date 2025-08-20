using Game.Utils;
using UnityEngine;

namespace Game.Controllers
{
    public class MissileHoming : MonoBehaviour
    {
        private MissileController missileController;
        private MissileMovement missileMovement;

        private Transform targetMeteor;
        private float lastTime = 0;
        private float homingRate = 0.5f;

        public void Initialize(MissileController missileController)
        {
            this.missileController = missileController;
            missileMovement = GetComponent<MissileMovement>();
        }

        public void Update()
        {
            if (Time.time - lastTime > homingRate) { 
                lastTime = Time.time;
                FingdClosestMeteor();               
            }

            if (targetMeteor != null)
            {
                NavigateDirection();
            }
        }

        private void FingdClosestMeteor()
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

        private void NavigateDirection()
        {
            Vector2 direction = (targetMeteor.position - transform.position).normalized;
            missileController.Rigidbody.velocity = direction * WeaponStats.missileSpeed;

            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}