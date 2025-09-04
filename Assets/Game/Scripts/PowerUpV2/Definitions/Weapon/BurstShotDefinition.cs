using UnityEngine;

namespace Game.PowerUpV2
{
    [CreateAssetMenu(fileName = "BurstShotDefinition", menuName = "PowerUps/Weapon/Burst Shot Definition")]
    public class BurstShotDefinition : PowerUpDefinition, IWeaponApplier
    {
        [SerializeField] private int baseBurstCount = 2;
        [SerializeField] private float baseBurstDelay = 0.05f;
        [SerializeField] private float levelScale = 1f;

        public int GetBurstCount(int level)
        {
            if (level < 1) level = 1;
            return baseBurstCount + (level - 1);
        }

        public float GetBurstDelay(int level)
        {
            if (level < 1) level = 1;
            return Mathf.Max(0f, baseBurstDelay - (level - 1) * levelScale * 0.01f);
        }

        public void ApplyToWeapon(GameObject weapon, int level)
        {
            var behavior = weapon.GetComponent<BurstShotBehavior>();
            if (behavior == null) behavior = weapon.AddComponent<BurstShotBehavior>();
            behavior.Init(GetBurstCount(level), GetBurstDelay(level));
            Debug.Log($"[PowerUpV2] Apply BurstShot(level={level}) to '{weapon.name}'");
        }

        public void RemoveFromWeapon(GameObject weapon)
        {
            var behavior = weapon.GetComponent<BurstShotBehavior>();
            bool had = behavior != null;
            if (had) Object.Destroy(behavior);
            Debug.Log($"[PowerUpV2] Remove BurstShot from '{weapon.name}', hadBehavior={had}");
        }
    }
}

