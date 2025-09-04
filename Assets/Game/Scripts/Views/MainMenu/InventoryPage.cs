using Game.PowerUp;
using Game.Scriptable;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Views
{
    public class InventoryPage : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Transform contentParent;
        [SerializeField] private GameObject powerUpItemPrefab;

        [Header("Data")]
        [SerializeField] private List<PowerUpData> powerUpDataList = new List<PowerUpData>();

        private void OnEnable()
        {
            BuildList();
        }

        public void BuildList()
        {
            if (contentParent == null || powerUpItemPrefab == null)
            {
                Debug.LogWarning("InventoryPage: Missing UI references.");
                return;
            }

            // Clear existing items
            for (int i = contentParent.childCount - 1; i >= 0; i--)
            {
                Destroy(contentParent.GetChild(i).gameObject);
            }

            // Ensure manager exists
            if (LevelPowerUpManager.Instance == null)
            {
                Debug.LogWarning("LevelPowerUpManager instance not found.");
                return;
            }

            // Build items in the order of provided data list
            foreach (var data in powerUpDataList)
            {
                if (data == null) continue;

                int currentLevel = LevelPowerUpManager.Instance.GetLevel(data.powerUpType);
                var go = Instantiate(powerUpItemPrefab, contentParent);
                var item = go.GetComponent<PowerUpItem>();
                if (item != null)
                {
                    item.Initialize(data, currentLevel, this);
                }
            }
        }
    }
}
