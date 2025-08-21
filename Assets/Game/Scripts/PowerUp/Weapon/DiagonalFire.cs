using Game.Controllers;
using UnityEngine;

namespace Game.PowerUp
{
    public class DiagonalFire : PowerUpEffect
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