using Game.Controllers;
using UnityEngine;

namespace Game.PowerUpV2
{
    public class InvisibleBehavior : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Color originalColor;
        private bool originalColorCaptured = false;
        private float targetAlpha = 0.5f;
        private PlayerStats playerStats;

        public void Init(float alpha)
        {
            targetAlpha = Mathf.Clamp01(alpha);
            Apply();
        }

        private void OnEnable()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            playerStats = GetComponent<PlayerStats>();
            Apply();
        }

        private void OnDisable()
        {
            Restore();
        }

        private void Apply()
        {
            if (spriteRenderer == null) return;
            if (!originalColorCaptured)
            {
                originalColor = spriteRenderer.color;
                originalColorCaptured = true;
            }
            var c = originalColor;
            spriteRenderer.color = new Color(c.r, c.g, c.b, targetAlpha);
            if (playerStats != null) playerStats.IsInvisible = true;
        }

        public void Restore()
        {
            if (spriteRenderer != null && originalColorCaptured)
            {
                spriteRenderer.color = originalColor;
            }
            if (playerStats != null) playerStats.IsInvisible = false;
        }
    }
}

