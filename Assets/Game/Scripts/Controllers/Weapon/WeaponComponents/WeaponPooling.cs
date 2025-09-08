using Game.PowerUp;
using Game.Services;
using System.Collections.Generic;
using UnityEngine;
using Game.PowerUpV2;

namespace Game.Controllers
{
    public class WeaponPooling : MonoBehaviour
    {


        private WeaponController weaponController;
        private PowerUpManager powerUpManager;
        public void Initialize(WeaponController weaponController)
        {
            this.weaponController = weaponController;
            CreatePool();

            // Try find PowerUpManager
            powerUpManager = GetComponentInParent<Game.PowerUpV2.PowerUpManager>();
            Debug.Log("PowerUpManager found in parent: " + (powerUpManager != null));
            if (powerUpManager == null)
            {
                var player = GameObject.FindWithTag("Player");
                powerUpManager = player != null ? player.GetComponent<Game.PowerUpV2.PowerUpManager>() : null;
                if (powerUpManager == null)
                {
                    Debug.LogWarning("PowerUpManager not found in WeaponPooling");
                }
            }
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
                missileController.Initialize("Missiles_" + missileIndex, direction);
                //missileController.SetVelocity(direction);

                if (powerUpManager != null)
                {
                    if (missile.GetComponent<MissilePowerUpSubscriber>() == null)
                    {
                        missile.AddComponent<MissilePowerUpSubscriber>();
                    }
                    foreach (var state in powerUpManager.GetActiveStates())
                    {
                        if (state.definition is Game.PowerUpV2.IMissileSpawnApplier applier)
                        {
                            applier.ApplyToMissile(missile, state.level);
                        }
                    }
                }
            }
        }
    }
}
