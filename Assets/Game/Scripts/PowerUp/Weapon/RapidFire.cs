using Game.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PowerUp
{
    public class RapidFire : PowerUpEffect
    {
        [SerializeField] private float fireRateMultiplier = 4f;
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

