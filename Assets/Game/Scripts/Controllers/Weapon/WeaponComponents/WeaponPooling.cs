using Game.Services;
using UnityEngine;

namespace Game.Controllers
{
    public class WeaponPooling : MonoBehaviour
    {
        private WeaponController weaponController;
        public void Initialize(WeaponController weaponController)
        {
            this.weaponController = weaponController;
            CreatePool();
        }

        private void CreatePool()
        {
            if (PoolManager.Instance != null)
            {
                for (int i = 0; i < weaponController.weaponData.missilePrefabs.Length; i++)
                {
                    PoolManager.Instance.CreatePool(
                        "Missiles_" + i,
                        weaponController.weaponData.missilePrefabs[i],
                        10,
                        30,
                        true);
                }
            }
        }

        public void SpawnMissile(int missileIndex, Vector3 position, Vector2 direction)
        {
            GameObject missile = PoolManager.Instance.Spawn("Missiles_" + missileIndex, position);
            if (missile != null)
            {
                var missileController = missile.GetComponent<MissileController>();
                missileController.Initialize("Missiles_" + missileIndex);
                missileController.SetVelocity(direction);
            }
        }
    }
}