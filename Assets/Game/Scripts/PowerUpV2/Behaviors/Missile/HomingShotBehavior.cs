using Game.Controllers;
using Game.Utils;
using UnityEngine;

namespace Game.PowerUpV2
{
    public class HomingShotBehavior : MonoBehaviour
    {
        private float homingRate = 0.5f;
        private float rotationSpeed = 5f;

        private Transform target;
        private float lastSeekTime;
        private MissileController missileController;
        private MissileStats missileStats;

        public void Init(HomingShotDefinition def, int level)
        {
            homingRate = def != null ? def.GetHomingRate(level) : 0.5f;
            rotationSpeed = def != null ? def.GetRotationSpeed(level) : 5f;
        }

        private void OnEnable()
        {
            missileController = GetComponent<MissileController>();
            missileStats = GetComponent<MissileStats>();
            lastSeekTime = Time.time;
        }

        private void FixedUpdate()
        {
            if (missileController == null || missileStats == null) return;

            if (Time.time - lastSeekTime >= homingRate)
            {
                lastSeekTime = Time.time;
                FindClosestTarget();
            }

            if (target != null)
            {
                NavigateToTarget();
            }
        }

        private void FindClosestTarget()
        {
            float closest = float.MaxValue;
            target = null;
            foreach (var entity in ChaseableEntity.AllEntities)
            {
                float d = Vector2.Distance(transform.position, entity.transform.position);
                if (d < closest)
                {
                    closest = d;
                    target = entity.transform;
                }
            }
        }

        private void NavigateToTarget()
        {
            Vector2 direction = (target.position - transform.position).normalized;
            missileController.Rigidbody.velocity = direction * missileStats.MissileSpeed;

            Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
        }
    }
}

