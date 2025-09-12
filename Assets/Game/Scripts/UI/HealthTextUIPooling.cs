using Game.Services;
using UnityEngine;

namespace Game.UI
{
    [DisallowMultipleComponent]
    public class HealthTextUIPooling : MonoBehaviour
    {
        public static HealthTextUIPooling Instance { get; private set; }

        [Header("Pool Config")] public string poolName = "HealthTextUI_Pool";
        public GameObject prefab;
        public int initialSize = 0;
        public int maxSize = 64;
        public bool autoExpand = true;

        [Header("Parent Canvas")] public Canvas hudCanvas;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;

            if (hudCanvas == null) hudCanvas = GetComponentInParent<Canvas>();
            if (PoolManager.Instance == null)
            {
                Debug.LogError("PoolManager.Instance missing. Add PoolManager to the scene first.");
                return;
            }
            if (prefab == null)
            {
                Debug.LogError("HealthTextUIPooling.prefab is not assigned.");
                return;
            }
            PoolManager.Instance.CreatePool(poolName, prefab, initialSize, maxSize, autoExpand);
        }
    }
}
