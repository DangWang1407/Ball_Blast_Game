using Game.Controllers;
using Game.Events;
using System.Collections;
using UnityEngine;

namespace Game.PowerUps
{
    public class ShieldPowerUp : IPowerUpDefend
    {
        public PowerUpType Type => PowerUpType.Shield;

        public void Apply(PlayerController controller, float duration)
        {
            var shield = controller.GetComponent<PlayerShield>();
            shield?.ActivateShield(duration);
        }
    }
}
