using UnityEngine;
using Game.Events;
using Game.Core;
using Game.Controllers;
using Game.PowerUpV2;
using System.Collections.Generic;

namespace Game.PowerUp
{
    public class PowerUpSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] powerUpPrefabs;

        private Dictionary<PowerUpType, GameObject> powerUpList = new Dictionary<PowerUpType, GameObject>();

        private void Awake()
        {
            Initialize();
            EventManager.Subscribe<PowerUpSpawnEvent>(OnPowerUpSpawn);
            EventManager.Subscribe<SpecificPowerUpSpawnEvent>(OnSpecificPowerUpSpawn);
        }

        private void Initialize()
        {
            foreach (var prefab in powerUpPrefabs)
            {
                //Debug.Log(prefab);
                var powerUpPickupSO = prefab.GetComponent<PowerUpPickupSO>();
                //Debug.Log(powerUpEffect);
                //Debug.Log(powerUpEffect.powerUpType);
                if(powerUpPickupSO != null)
                {
                    powerUpList[powerUpPickupSO.Definition.Type] = prefab;
                }
            }
        }

        private void OnSpecificPowerUpSpawn(SpecificPowerUpSpawnEvent e)
        {
            Instantiate(powerUpList[e.PowerUpType], e.Position, Quaternion.identity);
        }

        private void OnPowerUpSpawn(PowerUpSpawnEvent evt)
        {
            //Debug.Log($"Power-up spawn event received for meteor size: {evt.MeteorSize} at position: {evt.Position}");
            float dropChance = GetDropChance(evt.MeteorSize);
            //Debug.Log($"Drop chance for {evt.MeteorSize} meteor: {dropChance}");
            if (Random.Range(0f, 1f) < dropChance)
            {
                SpawnRandomPowerUp(evt.Position);
            }
        }

        private float GetDropChance(MeteorSize meteorSize)
        {
            return meteorSize switch
            {
                MeteorSize.Large => GameManager.Instance.Data.largeMeteorDropChance,
                MeteorSize.Medium => GameManager.Instance.Data.mediumMeteorDropChance,
                MeteorSize.Small => GameManager.Instance.Data.smallMeteorDropChance,
                _ => 0f
            };
        }

        private void SpawnRandomPowerUp(Vector3 position)
        {
            //Debug.Log($"Spawning power-up at {position}");
            var powerUpPrefabs = GameManager.Instance.Data.powerUpPrefabs;
            if (powerUpPrefabs.Length > 0)
            {
                int randomIndex = Random.Range(0, powerUpPrefabs.Length);
                Instantiate(powerUpPrefabs[randomIndex], position, Quaternion.identity);
            }
        }

        private void OnDestroy()
        {
            EventManager.Unsubscribe<PowerUpSpawnEvent>(OnPowerUpSpawn);
            EventManager.Unsubscribe<SpecificPowerUpSpawnEvent>(OnSpecificPowerUpSpawn);
        }
    }
}