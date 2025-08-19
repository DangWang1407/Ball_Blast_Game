using Game.Controllers;
using Game.Events;
using System.Collections;
using UnityEngine;

namespace Game.PowerUps
{
    public class InvisiblePowerUp : IPowerUpDefend
    {
        public PowerUpType Type => PowerUpType.Invisible;

        public void Apply(PlayerController controller, float duration)
        {
            var visuals = controller.GetComponent<PlayerVisuals>();
            visuals?.SetInvisible(duration);
        }
    }
}
