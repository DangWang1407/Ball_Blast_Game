using Game.Controllers;
using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PowerUp
{
    public class RapidFire : PowerUpEffect
    {
        private float fireRateMultiplier = 4f;              
        private float originalFireRate;
        
        private WeaponStats weaponStats;
        private RapidFireStats rapidFireStats;


        protected override void OnActivate()
        {
            //powerUpType = PowerUpType.RapidFire;

            //if (gameObject.CompareTag("PowerUp"))
            //{
            //    //currentLevel = LevelPowerUpManager.Instance.GetLevel(PowerUpType.RapidFire);

            //    rapidFireStats = GetComponent<RapidFireStats>();
                
            //    timer = rapidFireStats.GetDuration(currentLevel);
            //    fireRateMultiplier = rapidFireStats.GetFireRateMultiplier(currentLevel);

            //    Debug.Log(timer);
            //    Debug.Log(fireRateMultiplier);
            //}

            if (gameObject.CompareTag("Weapon"))
            {
                weaponStats = GetComponent<WeaponStats>();
                originalFireRate = weaponStats.FireRate;
                weaponStats.FireRate = originalFireRate / fireRateMultiplier;
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if(collision.CompareTag("Player"))
            {
                powerUpType = PowerUpType.RapidFire;

                if (gameObject.CompareTag("PowerUp"))
                {
                    currentLevel = levelPowerUpManager.GetLevel(PowerUpType.RapidFire);

                    rapidFireStats = GetComponent<RapidFireStats>();

                    timer = rapidFireStats.GetDuration(currentLevel);
                    fireRateMultiplier = rapidFireStats.GetFireRateMultiplier(currentLevel);

                    Debug.Log(timer);
                    Debug.Log(fireRateMultiplier);
                }
            }
            
        }

        protected override void OnDeactivate()
        {
            if (gameObject.CompareTag("Weapon"))
                weaponStats.FireRate = originalFireRate;
        }
    }
}

