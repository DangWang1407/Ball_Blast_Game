using UnityEngine;

namespace Game.PowerUpV2
{
    [CreateAssetMenu(fileName = "RapidFireDefinition", menuName = "PowerUps/Weapon/Rapid Fire Definition")]
    public class RapidFireDefinition : PowerUpDefinition, IWeaponApplier
    {
        [SerializeField] private float baseFireRateMultiplier = 4f;
        [SerializeField] private float levelScale = 1f;
        [SerializeField] private float perLevel = 0.1f;

        public float GetFireRateMultiplier(int level)
        {
            if (level < 1) level = 1;
            return baseFireRateMultiplier + (level - 1) * levelScale * perLevel;
        }

        public void ApplyToWeapon(GameObject weapon, int level)
        {
            var behavior = weapon.GetComponent<RapidFireBehavior>();
            if (behavior == null) behavior = weapon.AddComponent<RapidFireBehavior>();
            behavior.Init(GetFireRateMultiplier(level));
            Debug.Log($"[PowerUpV2] Apply RapidFire(level={level}) to '{weapon.name}'");
        }

        public void RemoveFromWeapon(GameObject weapon)
        {
            var behavior = weapon.GetComponent<RapidFireBehavior>();
            bool had = behavior != null;
            if (had) Object.Destroy(behavior);
            Debug.Log($"[PowerUpV2] Remove RapidFire from '{weapon.name}', hadBehavior={had}");
        }
    }
}

