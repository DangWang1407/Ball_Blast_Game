using Game.Controllers;
using UnityEngine;

namespace Game.PowerUp
{
    public class PowerShot : PowerUpEffect
    {
        private float orriginBulletScale;
        private MissileStats missileStats;

        protected override void OnActivate()
        {
            if (gameObject.CompareTag("Missile"))
            {
                missileStats = GetComponent<MissileStats>();
                orriginBulletScale = missileStats.BulletScale;
                missileStats.BulletScale *= 2;
            }
        }

        protected override void OnDeactivate()
        {
            if (gameObject.CompareTag("Missile"))
                missileStats.BulletScale = orriginBulletScale;
        }
    }
}