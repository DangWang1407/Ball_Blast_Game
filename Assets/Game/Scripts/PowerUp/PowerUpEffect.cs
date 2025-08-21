using UnityEngine;

namespace Game.PowerUp
{
    public abstract class PowerUpEffect : MonoBehaviour
    {
        [SerializeField] protected float duration = 20f;
        //[SerializeField] protected TargetType targetType = TargetType.Player;
        //public TargetType GetTargetType() { return targetType; }

        [SerializeField] protected float timer;

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
                Destroy(this);
            }
        }

        protected abstract void OnActivate();
        protected virtual void OnUpdate() { }
        protected abstract void OnDeactivate();
    } 

    
}