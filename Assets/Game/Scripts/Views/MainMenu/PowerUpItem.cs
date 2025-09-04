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
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Button upgradeButton;

        private InventoryPage inventoryPage;
        private PowerUpData data;

        public void Initialize(PowerUpData data, int currentLevel, InventoryPage inventorypage)
        {
            this.inventoryPage = inventorypage;
            this.data = data;

            // Populate UI
            if (nameText != null) nameText.text = data.powerUpName;
            if (descriptionText != null) descriptionText.text = data.powerUpDescription;
            if (levelText != null) levelText.text = $"Lv {currentLevel}";

            if (iconImage != null && data.powerUpImage != null)
            {
                // PowerUpData.powerUpImage is an Image; use its sprite if available
                iconImage.sprite = data.powerUpImage.sprite;
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

            // Directly call manager to upgrade
            LevelPowerUpManager.Instance.UpgradeLevel(data.powerUpType);

            // Refresh level text from manager without coin checks
            int newLevel = LevelPowerUpManager.Instance.GetLevel(data.powerUpType);
            if (levelText != null) levelText.text = $"Lv {newLevel}";
        }
    }
}
