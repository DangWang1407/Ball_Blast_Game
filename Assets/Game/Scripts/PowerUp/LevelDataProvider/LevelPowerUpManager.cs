using Game.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PowerUp
{
    public class LevelPowerUpManager : MonoBehaviour
    {
        Dictionary<PowerUpType, int> levelPowerUps = new Dictionary<PowerUpType, int>();


        #region Singleton
        public static LevelPowerUpManager Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion

        void Initialize()
        {
            // load data from json
            // if json is null
            foreach (PowerUpType powerUpType in System.Enum.GetValues(typeof(PowerUpType)))
            {
                levelPowerUps[powerUpType] = 1;
            }
        }

        public Dictionary<PowerUpType, int> GetLevelPowerUps()
        {
            return levelPowerUps;
        }

        public void UpgradeLevel(PowerUpType type)
        {
            if (!levelPowerUps.ContainsKey(type))
            {
                levelPowerUps[type] += 1;
            }
        }
    }
}