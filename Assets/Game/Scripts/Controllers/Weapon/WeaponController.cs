using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Services;
using Game.Events;
using Game.Scriptable;
using Game.PowerUps;

namespace Game.Controllers
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] public Transform firePoint;
        [SerializeField] public WeaponData weaponData;

        private float lastFireTime;
        private const string MISSILE_POOL = "Missiles";

        //private Dictionary<PowerUpType, IPowerUpWeapon> powerUps;

        private void Start()
        {
            if (firePoint == null)
            {
                Debug.LogError("FirePoint is not assigned in WeaponController.");
            }

            if (PoolManager.Instance != null)
            {
                for (int i = 0; i < weaponData.missilePrefabs.Length; i++)
                {
                    if (weaponData.missilePrefabs[i] != null)
                    {
                        PoolManager.Instance.CreatePool(
                            MISSILE_POOL + "_" + i,
                            weaponData.missilePrefabs[i],
                            10,
                            30,
                            true
                        );

                        Debug.Log($"Created pool for {MISSILE_POOL}_{i} with prefab {weaponData.missilePrefabs[i].name}");
                    }
                }
            }
        }

        private void Update()
        {
            if (Time.time - lastFireTime >= WeaponStats.fireRate)
            {
                FireMissile();
                lastFireTime = Time.time;
            }
        }

        private void FireMissile()
        {
            if (firePoint == null) firePoint = transform;

            if (WeaponStats.burst)
            {
                StartCoroutine(FireBurstMissiles());
            }
            else
            {
                FireSingleMissile(Vector2.up);
            }

            if (WeaponStats.diagonalFire)
            {
                FireSingleMissile(new Vector2(1, 1));
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

                string poolName = MISSILE_POOL + "_" + Mathf.Min(i, weaponData.missilePrefabs.Length - 1);

                GameObject missile = PoolManager.Instance.Spawn(poolName, spawnPos);
                if (missile != null)
                {
                    var missileController = missile.GetComponent<MissileController>();
                    missileController.Initialize(poolName);
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

        public IEnumerator ResetAfterDuration(float duration, System.Action resetAction)
        {
            yield return new WaitForSeconds(duration);
            resetAction?.Invoke();
        }

        private void OnDestroy()
        {
        }
    }
}
