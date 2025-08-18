using Game.Controllers;
using Game.Events;
using UnityEngine;

namespace Game.PowerUps
{
    public class BounceShotPowerUp : IPowerUpWeapon
    {
        public PowerUpType Type => PowerUpType.BounceShot;

        public void Apply(WeaponController controller, float duration)
        {
            WeaponStats.bounceShot = true;

            controller.StartCoroutine(controller.ResetAfterDuration(duration, () =>
            {
                WeaponStats.bounceShot = controller.weaponData.bounceShot;
            }));
        }
    }
}
