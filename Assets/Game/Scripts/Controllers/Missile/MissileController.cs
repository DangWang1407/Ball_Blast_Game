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
        // private MissileHoming missileHoming;
        private MissileStats missileStats;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Collider = GetComponent<Collider2D>();
            gameObject.tag = "Missile";

            missileMovement = GetComponent<MissileMovement>();
            missileCollision = GetComponent<MissileCollision>();
            missilePooling = GetComponent<MissilePooling>();
            // missileHoming = GetComponent<MissileHoming>();
            missileStats = GetComponent<MissileStats>();

            missileMovement.Initialize(this);
            missileCollision.Initialize(this);
            missilePooling.Initialize(this);
            // missileHoming.Initialize(this);
            missileStats.Initialize(this);

            Rigidbody.gravityScale = 0f;
            Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            Collider.isTrigger = true;

            // missileHoming.enabled = false;
        }

        public void Initialize(string poolName, Vector2 direction)
        {
            this.poolName = poolName;
            missileMovement.SetVelocity(direction);
        }

        public void OnCreate()
        {

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
            // missileHoming.enabled = missileStats.CanHoming;
        }
    }
}