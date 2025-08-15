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
        [SerializeField] private Transform firePoint;
        //[SerializeField] private float fireRate = 0.12f;
        //[SerializeField] private float missileSpeed = 8f;
        [SerializeField] private WeaponData weaponData;

        //need to fix 
        //private bool diagonalFire = true;
        Vector2[] directions;

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
                case PowerUpType.BurstShot:
                    ApplyBurstShot(powerUpEvent.Duration);
                    break;
                case PowerUpType.DamageBoost:
                    ApplyDamageBoost(powerUpEvent.Duration);
                    break;
                case PowerUpType.Homing:
                    ApplyHoming(powerUpEvent.Duration);
                    break;
                case PowerUpType.DiagonalFire:
                    ApplyDiagonalFire(powerUpEvent.Duration);
                    break;
                case PowerUpType.BounceShot:
                    ApplyBounceShot(powerUpEvent.Duration);
                    break;
            }
        }

        private void Update()
        {
            if(Time.time - lastFireTime >= WeaponStats.fireRate)
            {
                FireMissile();
                lastFireTime = Time.time;
            }
        }

        private void FireMissile()
        {
            if (firePoint == null) firePoint = transform;

            //if(diagonalFire)
            //{
            //    directions = new Vector2[]
            //    {
            //        new Vector2(1, 1).normalized,
            //        new Vector2(-1, 1).normalized,
            //    };
            //}
            //else
            //{
            //    directions = new Vector2[]
            //    {
            //        Vector2.up,
            //        Vector2.up,
            //    };
            //}


            if (WeaponStats.burst)
            {
                StartCoroutine(FireBurstMissiles());
            }
            else
            {
                FireSingleMissile(Vector2.up);
            }

            if(WeaponStats.diagonalFire)
            {
                FireSingleMissile(new Vector2(1,1));
                FireSingleMissile(new Vector2(-1, 1));
            }
        }

        private void FireSingleMissile(Vector2 direction)
        {
            for (int i = 0; i < WeaponStats.bulletCount; i++)
            {
                Vector3 spawnPos = firePoint.position;
                if (WeaponStats.bulletCount > 1)
                {
                    float offset = (i - (WeaponStats.bulletCount - 1) * 0.5f) * 0.3f;
                    spawnPos.x += offset;
                }

                GameObject missile = PoolManager.Instance.Spawn(MISSLE_POOL + "_" + i, spawnPos);
                if (missile != null)
                {
                    var missileController = missile.GetComponent<MissileController>();
                    missileController.Initialize(MISSLE_POOL + "_" + i);
                    missileController.SetVelocity(direction);
                }
            }
        }

        private IEnumerator FireBurstMissiles()
        {
            FireSingleMissile(Vector2.up);
            yield return new WaitForSeconds(WeaponStats.fireRate / 4f);
            FireSingleMissile(Vector2.up);
        }

        private void OnDestroy()
        {
            EventManager.Unsubscribe<PowerUpCollectedEvent>(OnPowerUpCollected);
        }

        #region Power-Up Methods
        public void ApplyRapidFire(float duration)
        {
            WeaponStats.fireRate = weaponData.fireRate / 2f;
            StartCoroutine(ResetAfterDuration(duration, () => 
            {
                WeaponStats.fireRate = weaponData.fireRate;
            }));
        }

        public void ApplyDoubleShot(float duration)
        {
            WeaponStats.bulletCount = 2;
            StartCoroutine(ResetAfterDuration(duration, () => 
            {
                WeaponStats.bulletCount = weaponData.bulletCount;
            }));
        }

        public void ApplyPowerShot(float duration)
        {
            WeaponStats.bulletScale = 1f;
            StartCoroutine(ResetAfterDuration(duration, () =>
            {
                WeaponStats.bulletScale = weaponData.bulletScale;
            }));
        }

        public void ApplyPierceShot(float duration)
        {
            WeaponStats.pierce = true;
            StartCoroutine(ResetAfterDuration(duration, () =>
            {
                WeaponStats.pierce = weaponData.pierce;
            }));
        }

        public void ApplyBurstShot(float duration)
        {
            WeaponStats.burst = true;
            StartCoroutine(ResetAfterDuration(duration, () =>
            {
                WeaponStats.burst = weaponData.burst;
            }));
        }

        public void ApplyDamageBoost(float duration)
        {
            WeaponStats.damage += 1;
            StartCoroutine(ResetAfterDuration(duration, () =>
            {
                WeaponStats.damage = weaponData.damage;
            }));
        }

        public void ApplyHoming(float duration)
        {
            WeaponStats.homing = true;
            StartCoroutine(ResetAfterDuration(duration, () =>
            {
                WeaponStats.homing = weaponData.homing;
            }));
        }

        public void ApplyDiagonalFire(float duration)
        {
            WeaponStats.diagonalFire = true;
            StartCoroutine(ResetAfterDuration(duration, () =>
            {
                WeaponStats.diagonalFire = weaponData.diagonalFire;
            }));
        }

        public void ApplyBounceShot(float duration)
        {
            WeaponStats.bounceShot = true;
            StartCoroutine(ResetAfterDuration(duration, () =>
            {
                WeaponStats.bounceShot = weaponData.bounceShot;
            }));
        }

        private IEnumerator ResetAfterDuration(float duration, System.Action resetAction)
        {
            yield return new WaitForSeconds(duration);
            resetAction?.Invoke();
        }
        #endregion
    }

    public static class WeaponStats
    {
        public static float missileSpeed = 6f;
        public static float fireRate = 0.2f;
        public static int bulletCount = 1;
        public static float bulletScale = 0.5f;
        public static bool pierce = false;
        public static bool burst = false;
        public static int damage = 1;
        public static bool homing = false;
        public static bool diagonalFire = false;
        public static bool bounceShot = false;
    }
}

