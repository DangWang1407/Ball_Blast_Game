using UnityEngine;
using Game.PowerUp;

namespace Game.PowerUpV2
{
    [CreateAssetMenu(fileName = "BounceShotDefinition", menuName = "PowerUps/Bounce Shot Definition")]
    public class BounceShotDefinition : PowerUpDefinition, IMissileSpawnApplier
    {
        [SerializeField] private int baseMaxBounces = 3;
        [SerializeField] private int bouncesPerLevel = 1;

        public int GetMaxBounces(int level)
        {
            if (level < 1) level = 1;
            return baseMaxBounces + (level - 1) * bouncesPerLevel;
        }

        public void ApplyToMissile(GameObject missile, int level)
        {
            var behavior = missile.GetComponent<BounceShotBehavior>();
            if (behavior == null) behavior = missile.AddComponent<BounceShotBehavior>();
            behavior.Init(this, level);

            Debug.Log($"[PowerUpV2] Apply BounceShot(level={level}) to '{missile.name}'");
        }

        public void RemoveFromMissile(GameObject missile)
        {
            var behavior = missile.GetComponent<BounceShotBehavior>();
            bool hadV2 = behavior != null;
            if (hadV2) Object.Destroy(behavior);

            Debug.Log($"[PowerUpV2] Remove BounceShot from '{missile.name}', hadV2={hadV2}");
        }
    }
}
