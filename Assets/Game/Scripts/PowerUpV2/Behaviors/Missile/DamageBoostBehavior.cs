using UnityEngine;
using Game.Controllers;

namespace Game.PowerUpV2
{
    public class DamageBoostBehavior : MonoBehaviour
    {
        private MissileStats missileStats;
        private int originalDamage;
        private float multiplier = 1f;

        public void Init(float damageMultiplier)
        {
            multiplier = Mathf.Max(0f, damageMultiplier);
            Apply();
        }

        private void OnEnable()
        {
            missileStats = GetComponent<MissileStats>();
            if (missileStats != null)
            {
                originalDamage = missileStats.Damage;
            }
            Apply();
        }

        private void OnDisable()
        {
            Restore();
        }

        private void Apply()
        {
            if (missileStats == null) missileStats = GetComponent<MissileStats>();
            if (missileStats == null) return;
            if (originalDamage <= 0) originalDamage = missileStats.Damage;
            missileStats.Damage = Mathf.RoundToInt(originalDamage * multiplier);
        }

        private void Restore()
        {
            if (missileStats == null) return;
            missileStats.Damage = originalDamage;
        }
    }
}

