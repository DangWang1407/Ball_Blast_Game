using UnityEngine;

namespace Game.Controllers
{
    public class NormalShot : MonoBehaviour
    {
        private WeaponShooting weaponShooting;

        public void Initialize()
        {
            weaponShooting = GetComponent<WeaponShooting>();
        }

        public void Fire()
        {
            weaponShooting.FireNormalMissile(Vector2.up);
        }
    }
}