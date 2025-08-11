using System.Runtime.CompilerServices;
using UnityEngine;
using Game.Services;

namespace Game.Controllers
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private GameObject missilePrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float fireRate = 0.12f;
        [SerializeField] private float missileSpeed = 8f;

        private float lastFireTime;
        private const string MISSLE_POOL = "Missiles";

        private void Start()
        {
            if(firePoint == null)
            {
                Debug.LogError("FirePoint is not assigned in WeaponController.");
            }
            if (missilePrefab == null)
            {
                Debug.LogError("Missile Prefab is not assigned in WeaponController.");
            }
            if (PoolManager.Instance != null)
            {
                PoolManager.Instance.CreatePool(MISSLE_POOL, missilePrefab, 5, 20, true);
            }
        }

        private void Update()
        {
            if(Time.time - lastFireTime >= fireRate)
            {
                FireMissile();
                lastFireTime = Time.time;
            }
        }

        private void FireMissile()
        {
            if(firePoint == null) firePoint = transform;

            GameObject missile = PoolManager.Instance.Spawn(MISSLE_POOL, firePoint.position);
            if (missile != null)
            {   
                var missileController = missile.GetComponent<MissileController>();
                missileController.Initialize(MISSLE_POOL, missileSpeed);
                missileController.SetVelocity(Vector2.up * missileSpeed);
            }
        }
    }
}