//using UnityEngine;
//using Game.Services;
//using Game.Events;
//using Game.Utils;

//namespace Game.Controllers
//{
//    public class  MissileController : MonoBehaviour, IPoolable 
//    {
//        public Rigidbody2D Rigidbody { get; private set; }
//        private Collider2D Collider { get; set; }
//        public bool IsActive { get => isActive; set => isActive = value; }

//        private string poolName;
//        //private float speed;
//        private bool isActive;


//        private Transform targetMeteor;

//        //need to fix
//        private bool bounceShot = false;
//        private int bounceLeft = 0;

//        //public WeaponStats weaponStats { get; private set; }

//        private void Awake()
//        {
//            Rigidbody = GetComponent<Rigidbody2D>();
//            Collider = GetComponent<Collider2D>();
//            gameObject.tag = "Missile";
//        }

//        public void Initialize(string poolName)
//        {
//            this.poolName = poolName;

//            transform.localScale = Vector3.one * WeaponStats.bulletScale;

//            bounceLeft = WeaponStats.bounceShot ? 3 : 0;
//        }

//        private void Update()
//        {
//            if (WeaponStats.homing)
//            {
//                FingdClosestMeteor();
//                if (targetMeteor != null)
//                {
//                    NavigateDirection();
//                }
//            }
//        }

//        private void FingdClosestMeteor()
//        {
//            float closestDistance = float.MaxValue;
//            targetMeteor = null;
//            foreach (var meteor in ChaseableEntity.AllEntities)
//            {
//                float distance = Vector2.Distance(transform.position, meteor.transform.position);
//                if (distance < closestDistance)
//                {
//                    closestDistance = distance;
//                    targetMeteor = meteor.transform;
//                }
//            }
//        }

//        private void NavigateDirection()
//        {
//            Vector2 direction = (targetMeteor.position - transform.position).normalized;
//            Rigidbody.velocity = direction * WeaponStats.missileSpeed;

//            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
//            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
//        }

//        public void OnCreate()
//        {
//            Rigidbody.gravityScale = 0f;
//            Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
//            Collider.isTrigger = true;
//        }
//        public void OnSpawned()
//        {
//            isActive = true;
//        }

//        public void OnDespawned()
//        {
//            isActive = false;
//            Rigidbody.velocity = Vector2.zero;
//        }

//        public void SetVelocity(Vector2 velocity)
//        {
//            Rigidbody.velocity = velocity * WeaponStats.missileSpeed;

//            //rotate base on velocity
//            if (velocity != Vector2.zero)
//            {
//                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90;
//                Rigidbody.rotation = angle;
//            }
//        }

//        public void DestroyMissile(MissileDestroyReason reason)
//        {
//            if(!isActive) return;
//            //EventManager.Trigger(new MissileDestroyedEvent(transform.position, reason));
//            PoolManager.Instance.Despawn(poolName, gameObject);
//        }

//        private void OnTriggerEnter2D(Collider2D other)
//        {
//            if(!isActive) return;
//            if (other.CompareTag("Wall"))
//            {
//                if(WeaponStats.bounceShot && bounceLeft > 0)
//                {
//                    bounceLeft--;
//                    Vector2 reflectDirection = Vector2.Reflect(Rigidbody.velocity.normalized, other.transform.right);
//                    SetVelocity(reflectDirection);
//                }
//                else
//                {
//                    DestroyMissile(MissileDestroyReason.OutOfBounds);
//                }
//            }
//            if(other.CompareTag("Ground") || other.CompareTag("UpBounce"))
//            {
//                if (WeaponStats.bounceShot && bounceLeft > 0)
//                {
//                    bounceLeft--;
//                    Vector2 reflectDirection = Vector2.Reflect(Rigidbody.velocity.normalized, other.transform.up);
//                    SetVelocity(reflectDirection);
//                }
//                else
//                {
//                    DestroyMissile(MissileDestroyReason.OutOfBounds);
//                }
//            }
//            if (other.CompareTag("Meteor") && !WeaponStats.pierce)
//            {
//                DestroyMissile(MissileDestroyReason.HitTarget);
//            }
//        }
//    }
//}

using UnityEngine;

namespace Game.Controllers
{
    public class MissileController : MonoBehaviour, IPoolable
    {
        public Rigidbody2D Rigidbody { get; private set; }
        private Collider2D Collider { get; set; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public string PoolName { get => poolName; private set => poolName = value; }

        private bool isActive;
        private string poolName;


        private MissileMovement missileMovement;
        private MissileCollision missileCollision;
        private MissilePooling missilePooling;
        private MissileHoming missileHoming;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Collider = GetComponent<Collider2D>();
            gameObject.tag = "Missile";

            missileMovement = GetComponent<MissileMovement>();
            missileCollision = GetComponent<MissileCollision>();
            missilePooling = GetComponent<MissilePooling>();
            missileHoming = GetComponent<MissileHoming>();

            missileMovement.Initialize(this);
            missileCollision.Initialize(this);
            missilePooling.Initialize(this);
            missileHoming.Initialize(this);

            Rigidbody.gravityScale = 0f;
            Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            Collider.isTrigger = true;

            missileHoming.enabled = false;
        }

        public void Initialize(string poolName, Vector2 direction)
        {
            this.poolName = poolName;
            missileMovement.SetVelocity(direction);
        }

        public void OnCreate()
        {
            //Rigidbody.gravityScale = 0f;
            //Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            //Collider.isTrigger = true;
        }

        public void OnSpawned()
        {
            isActive = true;
            missileCollision?.OnSpawned();
        }

        public void OnDespawned()
        {
            isActive = false;
            missileMovement?.OnDespawned();
        }

        private void Update()
        {
            missileHoming.enabled = WeaponStats.homing;
        }
    }
}