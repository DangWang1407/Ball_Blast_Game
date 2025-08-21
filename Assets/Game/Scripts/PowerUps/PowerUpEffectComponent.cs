using UnityEngine;

namespace Game.PowerUps
{
    public abstract class PowerUpEffectComponent : MonoBehaviour
    {
        [SerializeField] protected float duration = 20f;
        [SerializeField] protected bool autoDestroy = true;
        
        protected float timer;
        protected bool isActive;
        
        protected virtual void Start()
        {
            timer = duration;
            isActive = true;
            OnActivate();
        }
        
        protected virtual void Update()
        {
            if (!isActive) return;
            
            timer -= Time.deltaTime;
            OnUpdate();
            
            if (autoDestroy && timer <= 0f)
            {
                OnDeactivate();
                Destroy(this);
            }
        }
        
        protected abstract void OnActivate();
        protected virtual void OnUpdate() { }
        protected virtual void OnDeactivate() { }
        
        public void ForceDeactivate()
        {
            if (isActive)
            {
                OnDeactivate();
                isActive = false;
                Destroy(this);
            }
        }
    }
}
