using Game.Controllers;
using Game.Events;
using System.Collections;
using UnityEngine;

namespace Game.PowerUps
{
    public class RapidFirePowerUp : IPowerUpWeapon
    {
        public PowerUpType Type => PowerUpType.RapidFire;

        public void Apply(WeaponController controller, float duration)
        {
            WeaponStats.fireRate = controller.weaponData.fireRate / 2f;

            controller.StartCoroutine(controller.ResetAfterDuration(duration, () =>
            {
                WeaponStats.fireRate = controller.weaponData.fireRate;
            }));
        }
    }
}
