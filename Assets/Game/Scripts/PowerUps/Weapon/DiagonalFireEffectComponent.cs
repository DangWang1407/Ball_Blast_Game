using UnityEngine;
using Game.Controllers;

namespace Game.PowerUps.Weapon
{
    // Diagonal Fire Effect - Thay thế DiagonalShot component
    public class DiagonalFireEffectComponent : PowerUpEffectComponent
    {
        private bool originalDiagonalFire;
        private WeaponStats weaponStats;
        
        protected override void OnActivate()
        {
            weaponStats = GetComponent<WeaponStats>();
            if (weaponStats == null)
            {
                Debug.LogError("WeaponStats component not found on this GameObject!");
                return;
            }
            originalDiagonalFire = weaponStats.DiagonalFire;
            weaponStats.DiagonalFire = true;
        }
        
        protected override void OnDeactivate()
        {
            weaponStats.DiagonalFire = originalDiagonalFire;
        }
    }
}
