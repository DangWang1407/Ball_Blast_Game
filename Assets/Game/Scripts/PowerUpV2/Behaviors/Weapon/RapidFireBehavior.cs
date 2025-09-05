using UnityEngine;
using Game.Controllers;

namespace Game.PowerUpV2
{
    public class RapidFireBehavior : MonoBehaviour
    {
        private WeaponStats weaponStats;
        private float originalFireRate;
        private float multiplier = 1f;

        public void Init(float fireRateMultiplier)
        {
            multiplier = fireRateMultiplier;
            Apply();
        }

        private void OnEnable()
        {
            weaponStats = GetComponent<WeaponStats>();
            if (weaponStats != null)
            {
                originalFireRate = weaponStats.FireRate;
            }
            Apply();
        }

        private void OnDisable()
        {
            Restore();
        }

        private void Apply()
        {
            if (weaponStats == null) weaponStats = GetComponent<WeaponStats>();
            if (weaponStats == null) return;
            if (originalFireRate <= 0f) originalFireRate = weaponStats.FireRate;
            weaponStats.FireRate = originalFireRate / multiplier;
        }

        private void Restore()
        {
            if (weaponStats == null) return;
            weaponStats.FireRate = originalFireRate;
        }
    }
}

