using Game.Controllers;
using UnityEngine;

namespace Game.PowerUp
{
    public class PierceShot : PowerUpEffect
    {
        private bool originalPierce;
        private MissileStats missileStats;

        protected override void OnActivate()
        {
            if (gameObject.CompareTag("Missile"))
            {
                missileStats = GetComponent<MissileStats>();
                originalPierce = missileStats.CanPierce;
                missileStats.CanPierce = true;
                Debug.Log(missileStats.CanPierce);
            }
        }

        protected override void OnDeactivate()
        {
            if (gameObject.CompareTag("Missile"))
                missileStats.CanPierce = originalPierce;
        }
    }
}