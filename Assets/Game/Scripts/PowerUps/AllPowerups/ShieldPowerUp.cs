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
            controller.StartCoroutine(ShieldCoroutine(controller, duration));
        }

        private IEnumerator ShieldCoroutine(PlayerController controller, float duration)
        {
            if (controller.Shield == null)
                yield break;

            controller.Shield.SetActive(true);
            yield return new WaitForSeconds(duration);
            controller.Shield.SetActive(false);
        }
    }
}
