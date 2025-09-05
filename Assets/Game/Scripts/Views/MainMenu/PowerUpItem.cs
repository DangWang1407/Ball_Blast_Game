using Game.Scriptable;
using Game.PowerUp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public class PowerUpItem : MonoBehaviour
    {
        [Header("Bindings")]
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Button upgradeButton;

        private InventoryPage inventoryPage;
        private PowerUpData data;

        public void Initialize(PowerUpData data, int currentLevel, InventoryPage inventorypage)
        {
            this.inventoryPage = inventorypage;
            this.data = data;

            if (nameText != null) nameText.text = data.powerUpName;
            if (levelText != null) levelText.text = $"Lv {currentLevel}";

            if (iconImage != null)
            {
                iconImage.sprite = data.icon;
            }

            if (upgradeButton != null)
            {
                upgradeButton.onClick.RemoveAllListeners();
                upgradeButton.onClick.AddListener(OnUpgradeClicked);
            }
        }

        private void OnUpgradeClicked()
        {
            if (LevelPowerUpManager.Instance == null || data == null)
                return;

            LevelPowerUpManager.Instance.UpgradeLevel(data.powerUpType);

            int newLevel = LevelPowerUpManager.Instance.GetLevel(data.powerUpType);
            if (levelText != null) levelText.text = $"Lv {newLevel}";
        }
    }
}
