using Game.PowerUp;
using Game.Scriptable;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Views
{
    public class InventoryPage : MonoBehaviour
    {
        [SerializeField] private Transform contentParent;
        [SerializeField] private GameObject powerUpItemPrefab;


        [SerializeField] private List<PowerUpData> powerUpDataList = new List<PowerUpData>();

        private void Awake()
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

            if (LevelPowerUpManager.Instance == null)
            {
                Debug.LogWarning("LevelPowerUpManager instance not found.");
                return;
            }

            foreach (var data in powerUpDataList)
            {
                Debug.Log("Processing PowerUpData: " + (data != null ? data.powerUpName : "null"));
                if (data == null) continue;

                int currentLevel = LevelPowerUpManager.Instance.GetLevel(data.powerUpType);
                var go = Instantiate(powerUpItemPrefab, contentParent, false);

                // var rt = go.transform as RectTransform;
                // if (rt != null) rt.localScale = Vector3.one;
                var item = go.GetComponent<PowerUpItem>();
                if (item != null)
                {
                    item.Initialize(data, currentLevel, this);
                }
            }
        }
    }
}
