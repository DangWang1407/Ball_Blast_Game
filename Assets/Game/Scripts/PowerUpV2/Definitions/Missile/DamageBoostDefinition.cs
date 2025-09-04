using UnityEngine;

namespace Game.PowerUpV2
{
    [CreateAssetMenu(fileName = "DamageBoostDefinition", menuName = "PowerUps/Missile/Damage Boost Definition")]
    public class DamageBoostDefinition : PowerUpDefinition, IMissileSpawnApplier
    {
        [SerializeField] private float baseMultiplier = 1.5f;
        [SerializeField] private float levelScale = 1f;
        [SerializeField] private float perLevel = 0.2f;

        public float GetDamageMultiplier(int level)
        {
            if (level < 1) level = 1;
            return baseMultiplier + (level - 1) * levelScale * perLevel;
        }

        public void ApplyToMissile(GameObject missile, int level)
        {
            var behavior = missile.GetComponent<DamageBoostBehavior>();
            if (behavior == null) behavior = missile.AddComponent<DamageBoostBehavior>();
            behavior.Init(GetDamageMultiplier(level));
            Debug.Log($"[PowerUpV2] Apply DamageBoost(level={level}) to '{missile.name}'");
        }

        public void RemoveFromMissile(GameObject missile)
        {
            var behavior = missile.GetComponent<DamageBoostBehavior>();
            bool had = behavior != null;
            if (had) Object.Destroy(behavior);
            Debug.Log($"[PowerUpV2] Remove DamageBoost from '{missile.name}', hadBehavior={had}");
        }
    }
}

