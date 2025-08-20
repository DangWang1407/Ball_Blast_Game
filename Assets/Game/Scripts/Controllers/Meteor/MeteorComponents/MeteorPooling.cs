using Game.Services;
using UnityEngine;

namespace Game.Controllers
{
    public class MeteorPooling : MonoBehaviour
    {
        private MeteorController meteorController;
        private string poolName;

        public void Initialize(MeteorController meteorController)
        {
            this.meteorController = meteorController;
            poolName = meteorController.PoolName;
            Debug.Log("Pool name: " + poolName);
        }

        public void DestroyMeteor()
        {
            PoolManager.Instance.Despawn(poolName, gameObject);
        }
    }
}