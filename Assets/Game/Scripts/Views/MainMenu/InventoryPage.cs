using Game.PowerUp;
using Game.Scriptable;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public class InventoryPage : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private GameObject powerUpItemPrefab;


        [SerializeField] private List<PowerUpData> powerUpDataList = new List<PowerUpData>();

        private void Awake()
        {
            ConfigureScroll();
            BuildList();
        }

        private void ConfigureScroll()
        {
            if (scrollRect == null)
            {
                Debug.LogWarning("InventoryPage: Missing ScrollRect reference.");
                return;
            }

            scrollRect.vertical = true;
            scrollRect.horizontal = false;
        }

        public void BuildList()
        {
            foreach (var data in powerUpDataList)
            {
                Debug.Log("Processing PowerUpData: " + (data != null ? data.powerUpName : "null"));
                if (data == null) continue;

                int currentLevel = LevelPowerUpManager.Instance.GetLevel(data.powerUpType);
                var go = Instantiate(powerUpItemPrefab, scrollRect.content, false);

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
