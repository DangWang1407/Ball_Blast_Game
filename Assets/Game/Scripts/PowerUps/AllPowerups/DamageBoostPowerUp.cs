using Game.Controllers;
using Game.Events;
using UnityEngine;

namespace Game.PowerUps
{
    public class DamageBoostPowerUp : IPowerUpWeapon
    {
        public PowerUpType Type => PowerUpType.DamageBoost;

        public void Apply(WeaponController controller, float duration)
        {
            WeaponStats.damage += 1;

            controller.StartCoroutine(controller.ResetAfterDuration(duration, () =>
            {
                WeaponStats.damage = controller.weaponData.damage;
            }));
        }
    }
}
