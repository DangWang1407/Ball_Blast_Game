using UnityEngine;

namespace Game.PowerUpV2
{
    [CreateAssetMenu(fileName = "DoubleShotDefinition", menuName = "PowerUps/Weapon/Double Shot Definition")]
    public class DoubleShotDefinition : PowerUpDefinition, IWeaponApplier
    {
        [SerializeField] private int baseMultiplier = 2;
        [SerializeField] private float levelScale = 1f;
        [SerializeField] private int perLevel = 0; // optional extra per level

        public int GetMultiplier(int level)
        {
            if (level < 1) level = 1;
            return baseMultiplier + (level - 1) * (int)perLevel;
        }

        public void ApplyToWeapon(GameObject weapon, int level)
        {
            var behavior = weapon.GetComponent<DoubleShotBehavior>();
            if (behavior == null) behavior = weapon.AddComponent<DoubleShotBehavior>();
            behavior.Init(GetMultiplier(level));
            Debug.Log($"[PowerUpV2] Apply DoubleShot(level={level}) to '{weapon.name}'");
        }

        public void RemoveFromWeapon(GameObject weapon)
        {
            var behavior = weapon.GetComponent<DoubleShotBehavior>();
            bool had = behavior != null;
            if (had) Object.Destroy(behavior);
            Debug.Log($"[PowerUpV2] Remove DoubleShot from '{weapon.name}', hadBehavior={had}");
        }
    }
}

