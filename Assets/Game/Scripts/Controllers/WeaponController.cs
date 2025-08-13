using System.Runtime.CompilerServices;
using UnityEngine;
using Game.Services;
using System.Collections;

namespace Game.Controllers
{
    public class WeaponController : MonoBehaviour
    {
        public WeaponStats currentStats = new WeaponStats();
        private WeaponStats baseStats;



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

            baseStats = new WeaponStats
            {
                fireRate = fireRate,
                bulletCount = 1,
                bulletScale = 0.5f,
                pierce = false
            };
        }

        private void Update()
        {
            if(Time.time - lastFireTime >= currentStats.fireRate)
            {
                FireMissile();
                lastFireTime = Time.time;
            }
        }

        private void FireMissile()
        {
            if(firePoint == null) firePoint = transform;

            for (int i = 0; i < currentStats.bulletCount; i++)
            {
                Vector3 spawnPos = firePoint.position;
                if (currentStats.bulletCount > 1)
                {
                    float offset = (i - (currentStats.bulletCount - 1) * 0.5f) * 0.3f;
                    spawnPos.x += offset;
                }

                GameObject missile = PoolManager.Instance.Spawn(MISSLE_POOL, spawnPos);
                if (missile != null)
                {
                    var missileController = missile.GetComponent<MissileController>();
                    missileController.Initialize(MISSLE_POOL, missileSpeed, currentStats);
                    missileController.SetVelocity(Vector2.up * missileSpeed);
                }


            }
        }

        #region Power-Up Methods
        public void ApplyRapidFire(float duration)
        {
            currentStats.fireRate = baseStats.fireRate / 2f;
            StartCoroutine(ResetAfterDuration(duration, () => 
            {
                currentStats.fireRate = baseStats.fireRate;
            }));
        }

        public void ApplyDoubleShot(float duration)
        {
            currentStats.bulletCount = 2;
            StartCoroutine(ResetAfterDuration(duration, () => 
            {
                currentStats.bulletCount = baseStats.bulletCount;
            }));
        }

        public void ApplyPowerShot(float duration)
        {
            currentStats.bulletScale = 1f;
            StartCoroutine(ResetAfterDuration(duration, () =>
            {
                currentStats.bulletScale = baseStats.bulletScale;
            }));
        }

        public void ApplyPierceShot(float duration)
        {
            currentStats.pierce = true;
            StartCoroutine(ResetAfterDuration(duration, () =>
            {
                currentStats.pierce = baseStats.pierce;
            }));
        }

        private IEnumerator ResetAfterDuration(float duration, System.Action resetAction)
        {
            yield return new WaitForSeconds(duration);
            resetAction?.Invoke();
        }
        #endregion
    }

    public class WeaponStats
    {
        public float fireRate = 0.12f;
        public int bulletCount = 1;
        public float bulletScale = 0.5f;
        public bool pierce = false;
    }
}

