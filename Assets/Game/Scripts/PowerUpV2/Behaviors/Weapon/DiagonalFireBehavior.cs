using UnityEngine;
using Game.Controllers;

namespace Game.PowerUpV2
{
    public class DiagonalFireBehavior : MonoBehaviour
    {
        private WeaponShooting weaponShooting;
        private WeaponStats weaponStats;
        private float originalFireRate;
        private float fireRateMultiplier = 1.2f;
        private int extraMissileCount = 2;
        private float lastFireTime = 0f;

        public void Init(int extraCount, float fireRateMult)
        {
            extraMissileCount = Mathf.Max(0, extraCount);
            fireRateMultiplier = Mathf.Max(0.01f, fireRateMult);
            Apply();
        }

        private void OnEnable()
        {
            weaponShooting = GetComponent<WeaponShooting>();
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
            weaponStats.FireRate = originalFireRate / fireRateMultiplier;
        }

        private void Restore()
        {
            if (weaponStats == null) return;
            weaponStats.FireRate = originalFireRate;
        }

        private void FixedUpdate()
        {
            if (weaponShooting == null || weaponStats == null) return;
            if (Time.time - lastFireTime >= weaponStats.FireRate)
            {
                lastFireTime = Time.time;
                for (int i = 0; i < extraMissileCount; i++)
                {
                    float angle = 45f + (i * 90f);
                    Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                    weaponShooting.FireNormalMissile(direction);
                }
            }
        }
    }
}

