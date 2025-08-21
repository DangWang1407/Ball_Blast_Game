using Game.Controllers;
using UnityEngine;
namespace Game.PowerUp
{
    public class DoubleShot : PowerUpEffect
    {
        private int originalBulletCount;
        private WeaponStats weaponStats;

        protected override void OnActivate()
        {
            weaponStats = GetComponent<WeaponStats>();
            originalBulletCount = weaponStats.BulletCount;
            weaponStats.BulletCount *= 2;
        }

        protected override void OnDeactivate()
        {
            weaponStats.BulletCount /= 2;
        }
    }
}