using Game.Services;
using UnityEngine;

namespace Game.Controllers
{
    public class MeteorSpawnerPooling : MonoBehaviour
    {
        private MeteorSpawnerController controller;
        private readonly string[] poolNames = { "LargeMeteor_Pool", "MediumMeteor_Pool", "SmallMeteor_Pool" };

        public string[] PoolNames => poolNames;

        public void Initialize(MeteorSpawnerController controller)
        {
            this.controller = controller;
        }

        public void CreatePools()
        {
            for (int i = 0; i < controller.MeteorPrefabs.Length; i++)
            {
                PoolManager.Instance.CreatePool(poolNames[i], controller.MeteorPrefabs[i], 10, 30, true);
            }
        }

        public GameObject SpawnMeteor(int sizeIndex, Vector3 position)
        {
            return PoolManager.Instance.Spawn(poolNames[sizeIndex], position);
        }
    }
}