using UnityEngine;
using Game.Controllers;

namespace Game.PowerUps.Weapon
{   
    // Double Shot Effect
    public class DoubleShotEffectComponent : PowerUpEffectComponent
    {
        private int originalBulletCount;
        private WeaponStats weaponStats;
        
        protected override void OnActivate()
        {
            weaponStats = GetComponent<WeaponStats>();
            originalBulletCount = weaponStats.BulletCount;
            weaponStats.BulletCount *= 2;
        }
        
        protected override void OnDeactivate()
        {
            weaponStats.BulletCount = originalBulletCount;
        }
    }
}
