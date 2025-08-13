using System.Runtime.CompilerServices;
using UnityEngine;
using Game.Services;
using System.Collections;
using Game.Events;
using Game.Scriptable;

namespace Game.Controllers
{
    public class WeaponController : MonoBehaviour
    {
        public WeaponStats currentStats = new WeaponStats();
        private WeaponStats baseStats;

        //[SerializeField] private GameObject missilePrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float fireRate = 0.12f;
        [SerializeField] private float missileSpeed = 8f;

        [SerializeField] private WeaponData weaponData;

        private float lastFireTime;
        private const string MISSLE_POOL = "Missiles";

        private void Start()
        {
            if(firePoint == null)
            {
                Debug.LogError("FirePoint is not assigned in WeaponController.");
            }
            if (PoolManager.Instance != null)
            {
                //PoolManager.Instance.CreatePool(MISSLE_POOL, missilePrefab, 5, 20, true);
                for (int i = 0; i < weaponData.missilePrefabs.Length; i++)
                {
                    if (weaponData.missilePrefabs[i] != null)
                    {
                        PoolManager.Instance.CreatePool(MISSLE_POOL + "_" + i, weaponData.missilePrefabs[i], 10, 30, true);   
                        Debug.Log($"Created pool for {MISSLE_POOL}_{i} with prefab {weaponData.missilePrefabs[i].name}");
                    }
                }              
            }

            baseStats = new WeaponStats
            {
                fireRate = fireRate,
                bulletCount = 1,
                bulletScale = 0.5f,
                pierce = false
            };

            EventManager.Subscribe<PowerUpCollectedEvent>(OnPowerUpCollected);
        }

        private void OnPowerUpCollected(PowerUpCollectedEvent powerUpEvent)
        {
            switch (powerUpEvent.PowerUpType)
            {
                case PowerUpType.RapidFire:
                    ApplyRapidFire(powerUpEvent.Duration);
                    break;
                case PowerUpType.DoubleShot:
                    ApplyDoubleShot(powerUpEvent.Duration);
                    break;
                case PowerUpType.PowerShot:
                    ApplyPowerShot(powerUpEvent.Duration);
                    break;
                case PowerUpType.PierceShot:
                    ApplyPierceShot(powerUpEvent.Duration);
                    break;
            }
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

                GameObject missile = PoolManager.Instance.Spawn(MISSLE_POOL + "_" + i, spawnPos);
                if (missile != null)
                {
                    var missileController = missile.GetComponent<MissileController>();
                    missileController.Initialize(MISSLE_POOL + "_" + i,currentStats);
                    missileController.SetVelocity(Vector2.up * missileSpeed);
                }


            }
        }

        private void OnDestroy()
        {
            EventManager.Unsubscribe<PowerUpCollectedEvent>(OnPowerUpCollected);
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

