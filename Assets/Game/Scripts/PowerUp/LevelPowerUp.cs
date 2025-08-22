using Game.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PowerUp
{
    public class LevelPowerUp : MonoBehaviour
    {
        private Dictionary<PowerUpType, int> powerupLevels = new Dictionary<PowerUpType, int>();

        private void Initialize()
        {
            // load data from json
            // if json not found
            foreach (PowerUpType powerUpType in System.Enum.GetValues(typeof(PowerUpType))) {
                powerupLevels[powerUpType] = 1;
            }
            powerupLevels[PowerUpType.RapidFire] = 2;
        }

        public int GetLevel(PowerUpType type)
        {
            return powerupLevels.ContainsKey(type) ? powerupLevels[type] : 1;
        }

        public void UpgradeLevel(PowerUpType type)
        {
            if (!powerupLevels.ContainsKey(type))
            {
                powerupLevels[type] += 1;
            }
        }

        private void OnDestroy()
        {
            // save data to json
        }
    }
}