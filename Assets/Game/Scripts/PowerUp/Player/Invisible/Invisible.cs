using UnityEngine;

namespace Game.PowerUp
{
    public class Invisible : PowerUpEffect
    {
        private float alphaValue = 0.5f;
        private SpriteRenderer spriteRenderer;
        private Color originalColor;
        private InvisibleStats invisibleStats;

        protected override void OnActivate()
        {
            if (gameObject.CompareTag("Player"))
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    originalColor = spriteRenderer.color;
                    spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alphaValue);
                }
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (collision.CompareTag("Player"))
            {
                powerUpType = PowerUpType.Invisible;

                if (gameObject.CompareTag("PowerUp"))
                {
                    currentLevel = levelPowerUpManager.GetLevel(PowerUpType.Invisible);
                    invisibleStats = GetComponent<InvisibleStats>();

                    timer = invisibleStats.GetDuration(currentLevel);
                    alphaValue = invisibleStats.GetAlphaValue(currentLevel);
                }
            }
        }

        protected override void OnDeactivate()
        {
            if (gameObject.CompareTag("Player"))
                if (spriteRenderer != null)
                    {
                        spriteRenderer.color = originalColor;
                    }
        }
    }
}