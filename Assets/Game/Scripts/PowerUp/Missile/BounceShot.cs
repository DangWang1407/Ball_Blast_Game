using Game.Controllers;
using UnityEngine;

namespace Game.PowerUp
{
    public class BounceShot : PowerUpEffect
    {
        private bool originalBounceShot;
        private MissileStats missileStats;

        protected override void OnActivate()
        {
            if (gameObject.CompareTag("Missile"))
            {
                missileStats = GetComponent<MissileStats>();
                originalBounceShot = missileStats.CanBounce;
                missileStats.CanBounce = true;
            }
        }

        protected override void OnDeactivate()
        {
            if (gameObject.CompareTag("Missile"))
                missileStats.CanBounce = originalBounceShot;
        }
    }
}