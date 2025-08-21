using UnityEngine;
using Game.Controllers;
using Game.Utils;

namespace Game.PowerUps.Missile
{    
    // Bounce Shot Effect
    public class BounceEffectComponent : PowerUpEffectComponent
    {
        private bool originalBounceShot;
        private MissileStats missileStats;
        
        protected override void OnActivate()
        {
            missileStats = GetComponent<MissileStats>();
            autoDestroy = false; // Missile controls its own lifetime
            originalBounceShot = missileStats.CanBounce;
            missileStats.CanBounce = true;
        }
        
        protected override void OnDeactivate()
        {
            missileStats.CanBounce = originalBounceShot;
        }
    }
}