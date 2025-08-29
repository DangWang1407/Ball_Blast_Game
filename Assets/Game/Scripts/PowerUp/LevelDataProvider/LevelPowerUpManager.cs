using Game.Core;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Game.PowerUp
{
    [System.Serializable]
    public class PowerUpSaveData
    {
        public List<PowerUpLevel> powerUpLevels = new List<PowerUpLevel>();
    }

    [System.Serializable]
    public class PowerUpLevel
    {
        public PowerUpType type;
        public int level;
    }

    public class LevelPowerUpManager : MonoBehaviour
    {
        Dictionary<PowerUpType, int> levelPowerUps = new Dictionary<PowerUpType, int>();
        private string saveFileName = "powerup_levels.json";
        private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

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
            LoadData();
        }

        public void SaveData()
        {
            try
            {
                PowerUpSaveData saveData = new PowerUpSaveData();

                foreach (var kvp in levelPowerUps)
                {
                    saveData.powerUpLevels.Add(new PowerUpLevel
                    {
                        type = kvp.Key,
                        level = kvp.Value
                    });
                }

                string jsonData = JsonUtility.ToJson(saveData, true);
                File.WriteAllText(SavePath, jsonData);

                Debug.Log($"PowerUp levels saved to: {SavePath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to save: {e.Message}");
            }
        }

        public void LoadData()
        {
            try
            {
                if (File.Exists(SavePath))
                {
                    string jsonData = File.ReadAllText(SavePath);
                    PowerUpSaveData saveData = JsonUtility.FromJson<PowerUpSaveData>(jsonData);

                    levelPowerUps.Clear();

                    foreach (var powerUpLevel in saveData.powerUpLevels)
                    {
                        levelPowerUps[powerUpLevel.type] = powerUpLevel.level;
                    }

                    Debug.Log("Load successfully");
                }
                else
                {
                    InitializeDefaultValues();
                    SaveData(); 
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load: {e.Message}");
                InitializeDefaultValues();
            }
        }

        private void InitializeDefaultValues()
        {
            levelPowerUps.Clear();
            foreach (PowerUpType powerUpType in System.Enum.GetValues(typeof(PowerUpType)))
            {
                levelPowerUps[powerUpType] = 1;
            }
        }

        public Dictionary<PowerUpType, int> GetLevelPowerUps()
        {
            return new Dictionary<PowerUpType, int>(levelPowerUps); 
        }

        public int GetLevel(PowerUpType type)
        {
            return levelPowerUps.ContainsKey(type) ? levelPowerUps[type] : 1;
        }

        public void UpgradeLevel(PowerUpType type)
        {
            if (levelPowerUps.ContainsKey(type))
            {
                levelPowerUps[type]++;
            }
            else
            {
                levelPowerUps[type] = 2; 
            }

            SaveData(); 
            Debug.Log($"{type} upgraded to level {levelPowerUps[type]}");
        }

        public void SetLevel(PowerUpType type, int level)
        {
            levelPowerUps[type] = Mathf.Max(1, level); 
            SaveData();
        }

        void OnDestroy()
        {
            SaveData();
        }
    }
}