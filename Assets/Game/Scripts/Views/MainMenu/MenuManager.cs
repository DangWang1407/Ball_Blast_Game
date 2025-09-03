using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject battlePage;
        [SerializeField] private GameObject inventoryPage;

        [SerializeField] private Button battleButton;
        [SerializeField] private Button inventoryButton;

        void Start()
        {
            InitializeButtons();
            ShowPage(0);
        }

        void InitializeButtons()
        {
            if (battleButton != null)
                battleButton.onClick.AddListener(() => ShowPage(0));

            if (inventoryButton != null)
                inventoryButton.onClick.AddListener(() => ShowPage(1));
        }

        public void ShowPage(int pageIndex)
        {
            if (battlePage != null)
                battlePage.SetActive(false);
            if (inventoryPage != null)
                inventoryPage.SetActive(false);

            switch (pageIndex)
            {
                case 0: // Battle
                    if (battlePage != null)
                        battlePage.SetActive(true);
                    break;
                case 1: // Inventory
                    if (inventoryPage != null)
                        inventoryPage.SetActive(true);
                    break;
            }
        }
    }
}