using Game.Controllers;
using Game.Events;
using UnityEngine;

namespace Game.PowerUps
{
    public class PierceShotPowerUp : IPowerUpWeapon
    {
        public PowerUpType Type => PowerUpType.PierceShot;

        public void Apply(WeaponController controller, float duration)
        {
            WeaponStats.pierce = true;

            controller.StartCoroutine(controller.ResetAfterDuration(duration, () =>
            {
                WeaponStats.pierce = controller.weaponData.pierce;
            }));
        }
    }
}
