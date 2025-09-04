using UnityEngine;

namespace Game.PowerUpV2
{
    [CreateAssetMenu(fileName = "PierceShotDefinition", menuName = "PowerUps/Pierce Shot Definition")]
    public class PierceShotDefinition : PowerUpDefinition, IMissileSpawnApplier
    {
        public void ApplyToMissile(GameObject missile, int level)
        {
            var behavior = missile.GetComponent<PierceShotBehavior>();
            if (behavior == null) behavior = missile.AddComponent<PierceShotBehavior>();
            // No level-dependent state for Pierce in this basic version

            Debug.Log($"[PowerUpV2] Apply PierceShot(level={level}) to '{missile.name}'");
        }

        public void RemoveFromMissile(GameObject missile)
        {
            var behavior = missile.GetComponent<PierceShotBehavior>();
            bool hadV2 = behavior != null;
            if (hadV2) Object.Destroy(behavior);

            Debug.Log($"[PowerUpV2] Remove PierceShot from '{missile.name}', hadV2={hadV2}");
        }
    }
}
