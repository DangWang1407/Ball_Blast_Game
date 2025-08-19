using UnityEngine;

namespace Game.Controllers
{
    public class DiagonalShot : MonoBehaviour
    {
        private WeaponShooting weaponShooting;

        public void Initialize()
        {
            weaponShooting = GetComponent<WeaponShooting>();
        }

        public void Fire()
        {
            if (WeaponStats.diagonalFire)
            {
                weaponShooting.FireNormalMissile(new Vector2(1, 1));
                weaponShooting.FireNormalMissile(new Vector2(-1, 1));
            }
        }
    }
}