using UnityEngine;

namespace Game.PowerUpV2
{
    [CreateAssetMenu(fileName = "PowerShotDefinition", menuName = "PowerUps/Missile/Power Shot Definition")]
    public class PowerShotDefinition : PowerUpDefinition, IMissileSpawnApplier
    {
        [SerializeField] private float baseScaleMultiplier = 1.3f;
        [SerializeField] private float levelScale = 1f;
        [SerializeField] private float scalePerLevel = 0.2f;

        public float GetScaleMultiplier(int level)
        {
            if (level < 1) level = 1;
            return baseScaleMultiplier + (level - 1) * levelScale * scalePerLevel;
        }

        public void ApplyToMissile(GameObject missile, int level)
        {
            var behavior = missile.GetComponent<PowerShotBehavior>();
            if (behavior == null) behavior = missile.AddComponent<PowerShotBehavior>();
            behavior.Init(GetScaleMultiplier(level));

            Debug.Log($"[PowerUpV2] Apply PowerShot(level={level}) to '{missile.name}'");
        }

        public void RemoveFromMissile(GameObject missile)
        {
            var behavior = missile.GetComponent<PowerShotBehavior>();
            bool had = behavior != null;
            if (had)
            {
                Object.Destroy(behavior);
            }
            Debug.Log($"[PowerUpV2] Remove PowerShot from '{missile.name}', hadBehavior={had}");
        }
    }
}

