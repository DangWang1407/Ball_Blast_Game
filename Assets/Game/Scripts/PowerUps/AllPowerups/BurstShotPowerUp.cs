using Game.Controllers;
using Game.Events;
using UnityEngine;

namespace Game.PowerUps
{
    public class BurstShotPowerUp : IPowerUpWeapon
    {
        public PowerUpType Type => PowerUpType.BurstShot;

        public void Apply(WeaponController controller, float duration)
        {
            WeaponStats.burst = true;

            controller.StartCoroutine(controller.ResetAfterDuration(duration, () =>
            {
                WeaponStats.burst = controller.weaponData.burst;
            }));
        }
    }
}
