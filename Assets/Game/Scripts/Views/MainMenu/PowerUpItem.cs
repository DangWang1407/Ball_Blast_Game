using Game.Scriptable;
using Game.PowerUp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Game.Currency;

namespace Game.Views
{
    public class PowerUpItem : MonoBehaviour
    {
        [Header("Bindings")]
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private int upgradeCost = 50;

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

            // Not enough gold
            if (GoldManager.Instance != null && !GoldManager.Instance.CanSpend(upgradeCost))
            {
                return;
            }

            var btnRt = upgradeButton != null ? upgradeButton.GetComponent<RectTransform>() : null;
            var topBar = TopBar.Instance;

            void DoSpendAndUpgrade()
            {
                if (GoldManager.Instance == null || !GoldManager.Instance.Spend(upgradeCost))
                    return;

                LevelPowerUpManager.Instance.UpgradeLevel(data.powerUpType);
                int newLevel = LevelPowerUpManager.Instance.GetLevel(data.powerUpType);
                if (levelText != null) levelText.text = $"Lv {newLevel}";
            }

            if (topBar != null && btnRt != null)
            {
                topBar.PlaySpendEffectTo(btnRt, DoSpendAndUpgrade);
            }
            else
            {
                DoSpendAndUpgrade();
            }
        }
    }
}
