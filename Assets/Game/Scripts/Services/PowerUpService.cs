using UnityEngine;
using Game.Events;

namespace Game.Services
{
    public class  PowerUpService : MonoBehaviour 
    {
        private void Start()
        {
            EventManager.Subscribe<PowerUpSpawnEvent>(OnPowerUpSpawn);
        }

        private void OnPowerUpSpawn(PowerUpSpawnEvent powerUpSpawnEvent)
        {
            
        }

        private void OnDestroy()
        {
            EventManager.Unsubscribe<PowerUpSpawnEvent>(OnPowerUpSpawn);
        }
    }
}