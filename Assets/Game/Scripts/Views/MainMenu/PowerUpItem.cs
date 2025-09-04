using Game.Scriptable;
using TMPro;
using Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public class PowerUpItem : MonoBehaviour
    {
        private Image iconImage;
        private TMP_Text nameText;
        private TMP_Text levelText;
        private TMP_Text descriptionText;
        private Button upgrade;

        private InventoryPage inventoryPage;

        public void Initialize(PowerUpData data, InventoryPage inventorypage)
        {
            this.inventoryPage = inventorypage;


        }
    }
}