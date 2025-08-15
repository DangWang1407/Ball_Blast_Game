using UnityEngine;
using Game.Services;
using Game.Events;
using Game.Utils;

namespace Game.Controllers
{
    public class  MissileController : MonoBehaviour, IPoolable 
    {
        private Rigidbody2D rb;
        private Collider2D col;
        private string poolName;
        //private float speed;
        private bool isActive;

        private Transform targetMeteor;

        //need to fix
        //private bool homing = true;

        //public WeaponStats weaponStats { get; private set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            gameObject.tag = "Missile";
        }

        public void Initialize(string poolName)
        {
            this.poolName = poolName;
            //this.speed = speed;

            transform.localScale = Vector3.one * WeaponStats.bulletScale;
        }

        private void Update()
        {
            if (WeaponStats.homing)
            {
                FingdClosestMeteor();
                if (targetMeteor != null)
                {
                    NavigateDirection();
                }
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
            rb.velocity = direction * WeaponStats.missileSpeed;
            // Optional: Rotate the missile to face the target
            // This not working properly, need to fix
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

            //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //rb.rotation = angle;
        }

        public void OnCreate()
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            col.isTrigger = true;
        }
        public void OnSpawned()
        {
            isActive = true;
        }
        public void OnDespawned()
        {
            isActive = false;
            rb.velocity = Vector2.zero;
        }

        public void SetVelocity(Vector2 velocity)
        {
            rb.velocity = velocity * WeaponStats.missileSpeed;
        }

        public void DestroyMissile(MissileDestroyReason reason)
        {
            if(!isActive) return;
            //EventManager.Trigger(new MissileDestroyedEvent(transform.position, reason));
            PoolManager.Instance.Despawn(poolName, gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!isActive) return;
            if (other.CompareTag("Wall"))
            {
                DestroyMissile(MissileDestroyReason.OutOfBounds);
            }
            if(other.CompareTag("Ground"))
            {
                DestroyMissile(MissileDestroyReason.OutOfBounds);
            }
            if (other.CompareTag("Meteor") && !WeaponStats.pierce)
            {
                DestroyMissile(MissileDestroyReason.HitTarget);
            }
        }
    }
}