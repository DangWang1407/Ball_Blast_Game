using System.Collections;
using UnityEngine;

namespace Game.Controllers
{
    public class BurstShot : MonoBehaviour
    {
        private WeaponShooting weaponShooting;
        private WeaponStats weaponStats;

        public void Initialize()
        {
            weaponShooting = GetComponent<WeaponShooting>();
            weaponStats = GetComponent<WeaponStats>();
        }

        public void Fire()
        {
            if (weaponStats.Burst)
            {
                StartCoroutine(FireBurstMissiles());
            }
        }

        private IEnumerator FireBurstMissiles()
        {
            yield return new WaitForSeconds(weaponStats.FireRate / 4f);
            weaponShooting.FireNormalMissile(Vector2.up);
        }
    }
}