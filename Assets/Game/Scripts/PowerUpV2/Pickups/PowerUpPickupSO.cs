using UnityEngine;
using Game.PowerUpV2;

namespace Game.PowerUp
{
    // Optional pickup that uses ScriptableObject definition
    public class PowerUpPickupSO : MonoBehaviour
    {
        [SerializeField] private PowerUpDefinition definition;
        [SerializeField] private int level = 1;
        [SerializeField] private bool usePlayerLevel = true;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;

            var manager = collision.GetComponent<PowerUpManager>();
            if (manager != null && definition != null)
            {
                // if usePlayerLevel, pass 0 so manager reads LevelPowerUp
                int lvl = usePlayerLevel ? 0 : level;
                manager.Activate(definition, lvl);
            }

            Destroy(gameObject);
        }
    }
}

