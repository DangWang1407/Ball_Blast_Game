using Game.PowerUp;
using System.Collections.Generic;
using Unity;
using UnityEngine;

namespace Game.Views
{
    public class InventoryPage : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform container;
 
        Dictionary<PowerUpType, PowerUpItem> powerUpItems = new Dictionary<PowerUpType, PowerUpItem>();

        private void Initialize()
        {

        }

        private void Create(PowerUpItem item)
        {
            GameObject objItem = Instantiate(prefab, container);
            var powerUpItem = objItem.GetComponent<PowerUpItem>();
            if(powerUpItem == null)
            {
                powerUpItem = objItem.AddComponent<PowerUpItem>();
            }
        }
    }
}