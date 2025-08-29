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

        public void SetMeteorHealth(int health)
        {
            meteorHealth.SetCustomHealth(health);
            meteorUI?.UpdateHealthDisplay();
        }

        private void Update() => meteorMovement.UpdateRotation();

        private void FixedUpdate()
        {
            MeteorCollision.OnFixedUpdate();
        }

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