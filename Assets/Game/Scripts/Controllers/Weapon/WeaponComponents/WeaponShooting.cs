using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Controllers
{
    public class WeaponShooting : MonoBehaviour
    {
        //private WeaponController controller;
        private WeaponPooling weaponPooling;
        [SerializeField] public Transform FirePoint;

        private NormalShot normalShot;
        private BurstShot burstShot;
        private DiagonalShot diagonalShot;

        private float lastFireTime = 0;

        public void Initialize(WeaponController controller)
        {
            //this.controller = controller;
            weaponPooling = GetComponent<WeaponPooling>();
            normalShot = GetComponent<NormalShot>();
            burstShot = GetComponent<BurstShot>();
            diagonalShot = GetComponent<DiagonalShot>();

            //normalShot.Initialize();
            //burstShot.Initialize();
            //diagonalShot.Initialize();
            //weaponPooling.Initialize(controller);
        }

        //public void FireNormalMissile(Vector2 direction)
        //{
        //    for (int i = 0; i < WeaponStats.bulletCount; i++)
        //    {
        //        Vector3 spawnPos = FirePoint.position;

        //        if (WeaponStats.bulletCount > 1)
        //        {
        //            float offset = (i - (WeaponStats.bulletCount - 1) * 0.5f) * 0.3f;
        //            spawnPos.x += offset;
        //        }

        //        weaponPooling.SpawnMissile(i, spawnPos, direction);
        //    }
        //}

        public void FireNormalMissile(Vector2 direction)
        {
            StartCoroutine(FireBurstRoutine(direction));
        }

        private IEnumerator FireBurstRoutine(Vector2 direction)
        {
            for (int burst = 0; burst < WeaponStats.burstCount; burst++)
            {
                for (int i = 0; i < WeaponStats.bulletCount; i++)
                {
                    Vector3 spawnPos = FirePoint.position;

                    if (WeaponStats.bulletCount > 1)
                    {
                        float offset = (i - (WeaponStats.bulletCount - 1) * 0.5f) * 0.3f;
                        spawnPos.x += offset;
                    }

                    weaponPooling.SpawnMissile(i, spawnPos, direction);
                }
                if (burst < WeaponStats.burstCount - 1)
                    yield return new WaitForSeconds(WeaponStats.burstDelay);
            }
        }

        public void FixedUpdate()
        {
            if (Time.time - lastFireTime >= WeaponStats.fireRate)
            {
                lastFireTime = Time.time;
                normalShot.Fire();
                burstShot.Fire();
                diagonalShot.Fire();
                
            }
        }
    }
}