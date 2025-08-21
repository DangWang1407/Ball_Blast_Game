using UnityEngine;

namespace Game.Controllers
{
    public class DiagonalShot : MonoBehaviour
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
            if (weaponStats.DiagonalFire)
            {
                weaponShooting.FireNormalMissile(new Vector2(1, 1));
                weaponShooting.FireNormalMissile(new Vector2(-1, 1));
            }
        }
    }
}