using Game.Services;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Controllers
{
    public class MissilePooling : MonoBehaviour
    {
        private MissileController missileController;

        public void Initialize(MissileController missileController)
        {
            this.missileController = missileController;
        }

        public void DestroyMissile()
        {
            if(!missileController.IsActive)
            {
                return;
            }
            PoolManager.Instance.Despawn(missileController.PoolName, gameObject);
        }
    }
}