//using Game.Core;
//using Game.Events;
//using Game.Services;
//using Game.Controllers;
//using TMPro;
//using UnityEditor.EditorTools;
//using UnityEngine;

//public class MeteorController : MonoBehaviour, IPoolable
//{
//    [SerializeField] public Rigidbody2D rb;
//    [SerializeField] public TMP_Text TextHealth;
//    [SerializeField] private int maxHealth = 10;

//    private float jumpForce = 12f;

//    [SerializeField] private MeteorSize meteorSize = MeteorSize.Large;

//    private int currentHealth;
//    private string poolName;

//    public MeteorSize MeteorSize { get => meteorSize; set => meteorSize = value; }
//    public string PoolName { get => poolName; set => poolName = value; }

//    private void Awake()
//    {
//        if (rb == null)
//            rb = GetComponent<Rigidbody2D>();
//        gameObject.tag = "Meteor";
//       // Tạo chuyển động xoay ngẫu nhiên
//    }

//    public void Initialize(string poolName)
//    {
//        this.poolName = poolName;
//        ResetMeteor();
//    }

//    private void ResetMeteor()
//    {
//        currentHealth = maxHealth;
//        UpdateUI();

//        rb.mass = 1f;         // 
//        rb.drag = 0.2f;       // Giảm tốc chậm

//    }

//    void Update()
//    {
//        // Xoay meteor liên tục
//        rb.angularVelocity = 10f;
//    }

//    public void OnCreate() { }
//    public void OnSpawned()
//    {
//        ResetMeteor();
//    }
//    public void OnDespawned() { }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        //Debug.Log($"Meteor collided with {other.tag}");
//        if (other.CompareTag("Missile"))
//        {
//            var missileController = other.GetComponent<MissileController>();
//            TakeDamage(WeaponStats.damage);
//        }

//        if (other.CompareTag("Wall"))
//        {
//            float posX = transform.position.x;
//            if (posX > 0)
//                rb.velocity = new Vector2(-Mathf.Abs(rb.velocity.x), rb.velocity.y);
//            else
//                rb.velocity = new Vector2(Mathf.Abs(rb.velocity.x), rb.velocity.y);

//        }

//        if (other.CompareTag("Ground"))
//        {
//            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
//        }

//        if(other.CompareTag("UpBounce"))
//        {
//            rb.velocity = Vector2.Reflect(rb.velocity.normalized, other.transform.up);
//        }

//        if (other.CompareTag("Player"))
//        {
//            //var playerController = other.GetComponent<PlayerController>();
//            //if (playerController.IsInvisible) return;
//            Debug.Log("Meteor hit player");
//            // Trigger player death event
//            EventManager.Trigger(new PlayerDeathEvent(
//                    GameManager.Instance.Lives,
//                    DeathCause.MeteorHit
//                ));

//            // Destroy meteor sau khi hit player
//            PoolManager.Instance.Despawn(poolName, gameObject);
//            return;
//        }

//        if(other.CompareTag("Shield"))
//        {
//            Vector2 direction = (transform.position - other.transform.position).normalized;
//            rb.velocity = direction * 10f;
//            return;
//        }
//    }


//    public void SetMeteorSize(MeteorSize size)
//    {
//        meteorSize = size;
//        maxHealth = size == MeteorSize.Large ? 10 : size == MeteorSize.Medium ? 5 : 2;
//        currentHealth = maxHealth;
//        UpdateUI();
//    }

//    private void TakeDamage(int damage)
//    {
//        //Debug.Log($"Meteor of size {meteorSize} taking {damage} damage. Current health: {currentHealth}");
//        currentHealth -= damage;
//        UpdateUI();

//        if (currentHealth <= 0)
//        {
//            //Debug.Log($"Meteor of size {meteorSize} destroyed");
//            EventManager.Trigger(new ScoreChangeEvent(ScoreReason.MeteorDestroyed, 
//                meteorSize == MeteorSize.Large ? 100 : 
//                meteorSize == MeteorSize.Medium ? 50 : 25));
//            if (meteorSize != MeteorSize.Small)
//                MeteorSpawner.Instance?.SpawnSplitMeteors(transform.position, meteorSize);

//            PoolManager.Instance.Despawn(poolName, gameObject);

//            //EventManager.Trigger(new PowerUpSpawnEvent(transform.position, meteorSize));
//            //Debug.Log($"Power-up spawn event triggered for meteor size: {meteorSize} at position: {transform.position}");
//        }
//    }

//    private void UpdateUI()
//    {
//        if (TextHealth != null)
//            TextHealth.text = currentHealth.ToString();
//    }
//}


//public enum MeteorSize
//{
//    Large,
//    Medium,
//    Small
//}

using UnityEngine;
using Game.Events;
using Unity.VisualScripting;

namespace Game.Controllers
{
    public class MeteorController : MonoBehaviour, IPoolable
    {
        public Rigidbody2D Rigidbody { get; private set; }
        public MeteorSize MeteorSize { get => meteorSize; set => meteorSize = value; }
        public string PoolName { get => poolName; set => poolName = value; }

        private MeteorSize meteorSize;
        private string poolName;

        private MeteorMovement meteorMovement;
        private MeteorHealth meteorHealth;
        private MeteorPooling meteorPooling;
        private MeteorCollision MeteorCollision;
        private MeteorUI meteorUI;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            gameObject.tag = "Meteor";

            meteorMovement = gameObject.AddComponent<MeteorMovement>();
            meteorHealth = gameObject.AddComponent<MeteorHealth>();
            meteorPooling = gameObject.AddComponent<MeteorPooling>();
            MeteorCollision = gameObject.AddComponent<MeteorCollision>();
            meteorUI = gameObject.AddComponent<MeteorUI>();

            //meteorHealth = GetComponent<MeteorHealth>();
            //meteorPooling = GetComponent<MeteorPooling>();
            //MeteorCollision = GetComponent<MeteorCollision>();
            //meteorUI = GetComponent<MeteorUI>();

            meteorMovement.Initialize(this);
            meteorHealth.Initialize(this);
            //meteorPooling.Initialize(this);
            MeteorCollision.Initialize(this);
            meteorUI.Initialize(this);
        }

        public void Initialize(string poolName)
        {
            this.poolName = poolName;

            //meteorMovement.Initialize(this);
            //meteorHealth.Initialize(this);
            meteorPooling.Initialize(this);
            //MeteorCollision.Initialize(this);
            //meteorUI.Initialize(this);

            ResetMeteor();
        }

        private void ResetMeteor()
        {
            meteorHealth.ResetHealth();
            meteorMovement.ResetMovement();
            meteorUI.UpdateHealthDisplay();
        }

        public void SetMeteorSize(MeteorSize meteorSize)
        {
            MeteorSize = meteorSize;
            meteorHealth.SetHealthBySize(meteorSize);
            meteorUI?.UpdateHealthDisplay();
        }

        private void Update() => meteorMovement.UpdateRotation();

        public void OnCreate()
        {
            
        }

        public void OnSpawned()
        {
            ResetMeteor();
        }

        public void OnDespawned()
        {
            
        }
    }
}

public enum MeteorSize
{
    Large,
    Medium,
    Small
}