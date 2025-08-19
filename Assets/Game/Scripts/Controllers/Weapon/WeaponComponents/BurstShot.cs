using System.Collections;
using UnityEngine;

namespace Game.Controllers
{
    public class BurstShot : MonoBehaviour
    {
        private WeaponShooting weaponShooting;

        public void Initialize()
        {
            weaponShooting = GetComponent<WeaponShooting>();
        }

        public void Fire()
        {
            if (WeaponStats.burst)
            {
                StartCoroutine(FireBurstMissiles());
            }
        }

        private IEnumerator FireBurstMissiles()
        {
            yield return new WaitForSeconds(WeaponStats.fireRate / 4f);
            weaponShooting.FireNormalMissile(Vector2.up);
        }
    }
}