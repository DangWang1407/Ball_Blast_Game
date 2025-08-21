using UnityEngine;
using Game.Controllers;

namespace Game.PowerUps.Weapon
{
    // Damage Boost Effect
    public class DamageBoostEffectComponent : PowerUpEffectComponent
    {
        [SerializeField] private int damageBonus = 1;
        private int originalDamage;
        
        protected override void OnActivate()
        {
            // originalDamage = WeaponStats.damage;
            // WeaponStats.damage += damageBonus;
        }
        
        protected override void OnDeactivate()
        {
            // WeaponStats.damage = originalDamage;
        }
    }
}
