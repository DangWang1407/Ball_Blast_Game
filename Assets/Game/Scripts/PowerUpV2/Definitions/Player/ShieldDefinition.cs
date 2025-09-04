using UnityEngine;

namespace Game.PowerUpV2
{
    [CreateAssetMenu(fileName = "ShieldDefinition", menuName = "PowerUps/Player/Shield Definition")]
    public class ShieldDefinition : PowerUpDefinition, IPlayerApplier
    {
        [SerializeField] private GameObject shieldPrefab;

        public void ApplyToPlayer(GameObject player, int level)
        {
            var behavior = player.GetComponent<ShieldBehavior>();
            if (behavior == null) behavior = player.AddComponent<ShieldBehavior>();
            behavior.Init(shieldPrefab);

            Debug.Log($"[PowerUpV2] Apply Shield(level={level}) to '{player.name}'");
        }

        public void RemoveFromPlayer(GameObject player)
        {
            var behavior = player.GetComponent<ShieldBehavior>();
            bool hadBehavior = behavior != null;
            if (hadBehavior)
            {
                behavior.DestroyShield();
                Object.Destroy(behavior);
            }

            Debug.Log($"[PowerUpV2] Remove Shield from '{player.name}', hadBehavior={hadBehavior}");
        }
    }
}
