using UnityEngine;

namespace Game.PowerUpV2
{
    [CreateAssetMenu(fileName = "HomingShotDefinition", menuName = "PowerUps/Homing Shot Definition")]
    public class HomingShotDefinition : PowerUpDefinition, IMissileSpawnApplier
    {
        [SerializeField] private float baseHomingRate = 0.5f; 
        [SerializeField] private float baseRotationSpeed = 5f; 
        [SerializeField] private float levelScale = 1f;

        public float GetHomingRate(int level)
        {
            if (level < 1) level = 1;
            return Mathf.Max(0.1f, baseHomingRate - (level - 1) * levelScale * 0.05f);
        }

        public float GetRotationSpeed(int level)
        {
            if (level < 1) level = 1;
            return baseRotationSpeed + (level - 1) * levelScale * 1f;
        }

        public void ApplyToMissile(GameObject missile, int level)
        {
            var behavior = missile.GetComponent<HomingShotBehavior>();
            if (behavior == null) behavior = missile.AddComponent<HomingShotBehavior>();
            behavior.Init(this, level);

            Debug.Log($"[PowerUpV2] Apply Homing(level={level}) to '{missile.name}'");
        }

        public void RemoveFromMissile(GameObject missile)
        {
            var behavior = missile.GetComponent<HomingShotBehavior>();
            bool hadV2 = behavior != null;
            if (hadV2) Object.Destroy(behavior);

            Debug.Log($"[PowerUpV2] Remove Homing from '{missile.name}', hadV2={hadV2}");
        }
    }
}
