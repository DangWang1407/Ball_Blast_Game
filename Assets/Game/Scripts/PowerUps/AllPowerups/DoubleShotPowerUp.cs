using Game.Controllers;
using Game.Events;
using UnityEngine;

namespace Game.PowerUps
{
    public class DoubleShotPowerUp : IPowerUpWeapon
    {
        public PowerUpType Type => PowerUpType.DoubleShot;

        public void Apply(WeaponController controller, float duration)
        {
            WeaponStats.bulletCount = 2;

            controller.StartCoroutine(controller.ResetAfterDuration(duration, () =>
            {
                WeaponStats.bulletCount = controller.weaponData.bulletCount;
            }));
        }
    }
}
