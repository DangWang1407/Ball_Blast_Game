using Game.Controllers;
using Game.Events;
using UnityEngine;

namespace Game.PowerUps
{
    public class HomingPowerUp : IPowerUpWeapon
    {
        public PowerUpType Type => PowerUpType.Homing;

        public void Apply(WeaponController controller, float duration)
        {
            WeaponStats.homing = true;

            controller.StartCoroutine(controller.ResetAfterDuration(duration, () =>
            {
                WeaponStats.homing = controller.weaponData.homing;
            }));
        }
    }
}
