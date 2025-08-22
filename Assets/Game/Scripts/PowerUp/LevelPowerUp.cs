using Game.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PowerUp
{
    public class LevelPowerUp : MonoBehaviour
    {
        private Dictionary<PowerUpType, int> powerupLevels = new Dictionary<PowerUpType, int>();
        [SerializeField] private ProviderType provider;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            powerupLevels = LevelPowerUpFactory.GetData(provider);
        }

        public int GetLevel(PowerUpType type)
        {
            return powerupLevels.ContainsKey(type) ? powerupLevels[type] : 1;
        }

        private void OnDestroy()
        {
            // save data to json
        }
    }
}