using UnityEngine;
using Game.Controllers;

namespace Game.PowerUps.Weapon
{
    // Burst Shot Effect - Thay thế BurstShot component
    public class BurstShotEffectComponent : PowerUpEffectComponent
    {
        [SerializeField] private int burstCount = 3;
        [SerializeField] private float burstDelay = 0.1f;
        private bool originalBurst;
        private int originalBurstCount;
        private float originalBurstDelay;
        private WeaponStats weaponStats;
        
        protected override void OnActivate()
        {
            weaponStats = GetComponent<WeaponStats>();
            originalBurst = weaponStats.Burst;
            originalBurstCount = weaponStats.BurstCount;
            originalBurstDelay = weaponStats.BurstDelay;

            weaponStats.Burst = true;
            weaponStats.BurstCount = burstCount;
            weaponStats.BurstDelay = burstDelay;
        }
        
        protected override void OnDeactivate()
        {
            weaponStats.Burst = originalBurst;
            weaponStats.BurstCount = originalBurstCount;
            weaponStats.BurstDelay = originalBurstDelay;
        }
    }
}
