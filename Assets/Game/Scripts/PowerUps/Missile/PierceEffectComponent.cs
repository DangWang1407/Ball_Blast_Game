using UnityEngine;
using Game.Controllers;
using Game.Utils;

namespace Game.PowerUps.Missile
{   
    // Pierce Shot Effect
    public class PierceEffectComponent : PowerUpEffectComponent
    {
        private bool originalPierce;
        private MissileStats missileStats;
        
        protected override void OnActivate()
        {
            missileStats = GetComponent<MissileStats>();
            autoDestroy = false; // Missile controls its own lifetime
            originalPierce = missileStats.CanPierce;
            missileStats.CanPierce = true;
        }
        
        protected override void OnDeactivate()
        {
            missileStats.CanPierce = originalPierce;
        }
    }
}