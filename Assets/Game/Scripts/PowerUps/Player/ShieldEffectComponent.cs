using UnityEngine;
using System.Collections;

namespace Game.PowerUps.Player
{
    // Shield Effect - Thay thế PlayerShield
    public class ShieldEffectComponent : PowerUpEffectComponent
    {
        [SerializeField] private GameObject shieldPrefab;
        private GameObject shield;
        
        protected override void OnActivate()
        {
            CreateAndActivateShield();
        }
        
        protected override void OnDeactivate()
        {
            if (shield != null)
            {
                shield.SetActive(false);
                Destroy(shield);
            }
        }
        
        private void CreateAndActivateShield()
        {
            if (shieldPrefab != null)
            {
                shield = Instantiate(shieldPrefab, transform);
                shield.transform.localPosition = Vector3.zero;
                shield.tag = "Shield";
                shield.SetActive(true);
            }
        }
    }
}
