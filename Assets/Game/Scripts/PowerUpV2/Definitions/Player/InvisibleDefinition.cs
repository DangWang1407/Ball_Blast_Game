using UnityEngine;

namespace Game.PowerUpV2
{
    [CreateAssetMenu(fileName = "InvisibleDefinition", menuName = "PowerUps/Player/Invisible Definition")]
    public class InvisibleDefinition : PowerUpDefinition, IPlayerApplier
    {
        [SerializeField] private float baseAlpha = 0.5f;
        [SerializeField] private float levelScale = 1.0f;
        [SerializeField] private float alphaPerLevel = 0.1f;
        [SerializeField] private float minAlpha = 0.1f;

        public float GetAlpha(int level)
        {
            if (level < 1) level = 1;
            float a = baseAlpha - (level - 1) * levelScale * alphaPerLevel;
            return Mathf.Clamp(a, minAlpha, 1f);
        }

        public void ApplyToPlayer(GameObject player, int level)
        {
            var behavior = player.GetComponent<InvisibleBehavior>();
            if (behavior == null) behavior = player.AddComponent<InvisibleBehavior>();
            behavior.Init(GetAlpha(level));
        }

        public void RemoveFromPlayer(GameObject player)
        {
            var behavior = player.GetComponent<InvisibleBehavior>();
            bool hadBehavior = behavior != null;
            if (hadBehavior)
            {
                behavior.Restore();
                Object.Destroy(behavior);
            }
        }
    }
}

