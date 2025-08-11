using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.Pool;
using Unity.Collections;

namespace Game.Services
{
    public class PoolManager : MonoBehaviour 
    {
        [System.Serializable]
        public class  PoolConfig
        {
            public string poolName;
            public GameObject prefab;
            public int initialSize;
            public int maxSize;
            public bool autoExpand;
        }

        [SerializeField]
        private PoolConfig[] poolConfigs;
        private Dictionary<string, ObjectPool> pools = new Dictionary<string, ObjectPool>();

        #region Singleton
        public static PoolManager Instance { get; private set; }

        private void Awake()
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

        private void Initialize()
        {
            foreach (var config in poolConfigs)
            {
                if (config.prefab == null)
                {
                    Debug.LogError($"Prefab for pool '{config.poolName}' is not assigned.");
                    continue;
                }
                CreatePool(config.poolName, config.prefab, config.initialSize, config.maxSize, config.autoExpand);
            }
        }

        public void CreatePool(string poolName, GameObject prefab, int initialSize, int maxSize, bool autoExpand)
        {
            if (pools.ContainsKey(poolName))
            {
                Debug.LogWarning($"Pool '{poolName}' already exists. Skipping creation.");
                return;
            }
            Transform container = new GameObject($"{poolName}_Container").transform;
            pools[poolName] = new ObjectPool(poolName, prefab, container, initialSize, maxSize, autoExpand);
        }
        #endregion

        public GameObject Spawn (string poolName, Vector3 position, Quaternion rotation = default, Transform parent = null)
        {
           if(!pools.ContainsKey(poolName))
           {
               Debug.LogError($"Pool '{poolName}' does not exist.");
               return null;
           }
           return pools[poolName].Get(position, rotation, parent);
        }

        public void Despawn(string poolName, GameObject obj)
        {
            if (!pools.ContainsKey(poolName))
            {
                Debug.LogError($"Pool '{poolName}' does not exist.");
                return;
            }
            pools[poolName].Return(obj);
        }
    }
}