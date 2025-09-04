using UnityEngine;

namespace Game.PowerUpV2
{
    [CreateAssetMenu(fileName = "DiagonalFireDefinition", menuName = "PowerUps/Weapon/Diagonal Fire Definition")]
    public class DiagonalFireDefinition : PowerUpDefinition, IWeaponApplier
    {
        [SerializeField] private int baseExtraMissileCount = 2;
        [SerializeField] private float baseFireRateMultiplier = 1.2f;
        [SerializeField] private float levelScale = 1f;

        public int GetExtraMissileCount(int level)
        {
            if (level < 1) level = 1;
            return baseExtraMissileCount; // can scale if needed later
        }

        public float GetFireRateMultiplier(int level)
        {
            if (level < 1) level = 1;
            return baseFireRateMultiplier + (level - 1) * levelScale * 0.05f;
        }

        public void ApplyToWeapon(GameObject weapon, int level)
        {
            var behavior = weapon.GetComponent<DiagonalFireBehavior>();
            if (behavior == null) behavior = weapon.AddComponent<DiagonalFireBehavior>();
            behavior.Init(GetExtraMissileCount(level), GetFireRateMultiplier(level));
            Debug.Log($"[PowerUpV2] Apply DiagonalFire(level={level}) to '{weapon.name}'");
        }

        public void RemoveFromWeapon(GameObject weapon)
        {
            var behavior = weapon.GetComponent<DiagonalFireBehavior>();
            bool had = behavior != null;
            if (had) Object.Destroy(behavior);
            Debug.Log($"[PowerUpV2] Remove DiagonalFire from '{weapon.name}', hadBehavior={had}");
        }
    }
}

