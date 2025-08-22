using UnityEngine;

namespace Game.PowerUp
{
    public abstract class PowerUpEffect : MonoBehaviour
    {
        protected PowerUpType powerUpType;
        private float duration = 20f;
        protected float timer;
        protected int currentLevel = 1;
        protected LevelPowerUp levelPowerUpManager;
        
        protected virtual void Awake()
        {
            //enabled = false;
        }

        protected virtual void Start()
        {
            timer = duration;
            OnActivate();
        }

        protected virtual void Update()
        {
            timer -= Time.deltaTime;
            OnUpdate();
            if(timer <= 0)
            {
                OnDeactivate();
                Debug.Log("Effect is destroyed");
                Destroy(this);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                levelPowerUpManager = other.gameObject.GetComponent<LevelPowerUp>();
            }
        }

        protected abstract void OnActivate();
        protected virtual void OnUpdate() { }
        protected abstract void OnDeactivate();
    } 
}