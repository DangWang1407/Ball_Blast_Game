using Game.Controllers;
using Game.Events;
using UnityEngine;

namespace Game.PowerUps
{
    public class DiagonalFirePowerUp : IPowerUpWeapon
    {
        public PowerUpType Type => PowerUpType.DiagonalFire;

        public void Apply(WeaponController controller, float duration)
        {
            WeaponStats.diagonalFire = true;

            controller.StartCoroutine(controller.ResetAfterDuration(duration, () =>
            {
                WeaponStats.diagonalFire = controller.weaponData.diagonalFire;
            }));
        }
    }
}
