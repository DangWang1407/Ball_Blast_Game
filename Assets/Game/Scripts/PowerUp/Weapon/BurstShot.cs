using Game.Controllers;
using UnityEngine;

namespace Game.PowerUp
{
    public class BurstShot : PowerUpEffect
    {
        [SerializeField] private int burstCount = 3;
        [SerializeField] private float burstDelay = 0.05f;
        //private bool originalBurst;
        private int originalBurstCount;
        private float originalBurstDelay;
        private WeaponStats weaponStats;

        protected override void OnActivate()
        {
            weaponStats = GetComponent<WeaponStats>();
            //originalBurst = weaponStats.Burst;
            originalBurstCount = weaponStats.BurstCount;
            originalBurstDelay = weaponStats.BurstDelay;

            //weaponStats.Burst = true;
            weaponStats.BurstCount = burstCount;
            weaponStats.BurstDelay = burstDelay;
        }

        protected override void OnDeactivate()
        {
            //weaponStats.Burst = originalBurst;
            weaponStats.BurstCount = originalBurstCount;
            weaponStats.BurstDelay = originalBurstDelay;
        }
    }
}