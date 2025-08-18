using Game.Controllers;
using Game.Events;
using UnityEngine;

namespace Game.PowerUps
{
    public class PowerShotPowerUp : IPowerUpWeapon
    {
        public PowerUpType Type => PowerUpType.PowerShot;

        public void Apply(WeaponController controller, float duration)
        {
            WeaponStats.bulletScale = 1f;

            controller.StartCoroutine(controller.ResetAfterDuration(duration, () =>
            {
                WeaponStats.bulletScale = controller.weaponData.bulletScale;
            }));
        }
    }
}
