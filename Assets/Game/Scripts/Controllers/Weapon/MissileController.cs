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
        private bool bounceShot = false;
        private int bounceLeft = 0;

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

            transform.localScale = Vector3.one * WeaponStats.bulletScale;

            bounceLeft = WeaponStats.bounceShot ? 3 : 0;
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

            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
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

            //rotate base on velocity
            if (velocity != Vector2.zero)
            {
                //fix this, this is not working properly
                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90;
                rb.rotation = angle;
            }
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
                if(WeaponStats.bounceShot && bounceLeft > 0)
                {
                    bounceLeft--;
                    Vector2 reflectDirection = Vector2.Reflect(rb.velocity.normalized, other.transform.right);
                    SetVelocity(reflectDirection);
                }
                else
                {
                    DestroyMissile(MissileDestroyReason.OutOfBounds);
                }
            }
            if(other.CompareTag("Ground") || other.CompareTag("UpBounce"))
            {
                if (WeaponStats.bounceShot && bounceLeft > 0)
                {
                    bounceLeft--;
                    Vector2 reflectDirection = Vector2.Reflect(rb.velocity.normalized, other.transform.up);
                    SetVelocity(reflectDirection);
                }
                else
                {
                    DestroyMissile(MissileDestroyReason.OutOfBounds);
                }
            }
            if (other.CompareTag("Meteor") && !WeaponStats.pierce)
            {
                DestroyMissile(MissileDestroyReason.HitTarget);
            }
        }
    }
}