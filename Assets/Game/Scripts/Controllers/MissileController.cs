using UnityEngine;
using Game.Services;
using Game.Events;

namespace Game.Controllers
{
    public class  MissileController : MonoBehaviour, IPoolable 
    {
        private Rigidbody2D rb;
        private string poolName;
        private float speed;
        private bool isActive;

        private WeaponStats weaponStats;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            gameObject.tag = "Missile";
        }

        public void Initialize(string poolName, float speed, WeaponStats weaponStats)
        {
            this.poolName = poolName;
            this.speed = speed;
            this.weaponStats = weaponStats;

            transform.localScale = Vector3.one * weaponStats.bulletScale;
        }

        public void OnCreate()
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            GetComponent<Collider2D>().isTrigger = true;
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
            rb.velocity = velocity;
        }

        public void DestroyMissile(MissileDestroyReason reason)
        {
            if(!isActive) return;
            EventManager.Trigger(new MissileDestroyedEvent(transform.position, reason));
            PoolManager.Instance.Despawn(poolName, gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!isActive) return;
            if (other.CompareTag("Wall"))
            {
                DestroyMissile(MissileDestroyReason.OutOfBounds);
            }
        }
    }
}