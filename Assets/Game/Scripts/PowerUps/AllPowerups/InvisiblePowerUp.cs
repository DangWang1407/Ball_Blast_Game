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
            controller.StartCoroutine(InvisibleCoroutine(controller, duration));
        }

        private IEnumerator InvisibleCoroutine(PlayerController controller, float duration)
        {
            controller.IsInvisible = true;
            var spriteRenderer = controller.GetComponent<SpriteRenderer>();
            Color originalColor = spriteRenderer.color;

            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
            yield return new WaitForSeconds(duration);

            spriteRenderer.color = originalColor;
            controller.IsInvisible = false;
        }
    }
}
