using UnityEngine;
using System.Collections;

namespace Game.PowerUps.Player
{
    // Invisible Effect - Thay thế PlayerVisuals.SetInvisible
    public class InvisibleEffectComponent : PowerUpEffectComponent
    {
        [SerializeField] private float alphaValue = 0.5f;
        private SpriteRenderer spriteRenderer;
        private Color originalColor;
        
        protected override void OnActivate()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alphaValue);
            }
        }
        
        protected override void OnDeactivate()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
        }
    }
}
