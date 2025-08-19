using System.Collections;
using UnityEngine;

namespace Game.Controllers
{
    public class PlayerVisuals : MonoBehaviour
    {
        private PlayerController controller;
        private SpriteRenderer spriteRenderer;
        private Color originalColor;

        public bool IsInvisible { get; private set; }

        public void Initialize(PlayerController controller)
        {
            this.controller = controller;
            spriteRenderer = GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.color;
        }

        public void SetInvisible(float duration)
        {
            StartCoroutine(InvisibleCoroutine(duration));
        }

        private IEnumerator InvisibleCoroutine(float duration)
        {
            IsInvisible = true;
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);

            yield return new WaitForSeconds(duration);

            spriteRenderer.color = originalColor;
            IsInvisible = false;
        }
    }
}