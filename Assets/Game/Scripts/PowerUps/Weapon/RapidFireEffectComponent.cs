using UnityEngine;
using Game.Controllers;

namespace Game.PowerUps.Weapon
{
    // Rapid Fire Effect
    public class RapidFireEffectComponent : PowerUpEffectComponent
    {
        [SerializeField] private float fireRateMultiplier = 2f;
        private float originalFireRate;
        private WeaponStats weaponStats;
        
        protected override void OnActivate()
        {
            weaponStats = GetComponent<WeaponStats>();
            originalFireRate = weaponStats.FireRate;
            weaponStats.FireRate = originalFireRate / fireRateMultiplier;
        }
        
        protected override void OnDeactivate()
        {
            weaponStats.FireRate = originalFireRate;
        }
    }
}
