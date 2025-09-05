using UnityEngine;
using Game.PowerUpV2;
using Game.PowerUp;
using Game.Events;

namespace Game.Controllers
{
    // Attach on missile to remove applied power-up behaviors when they expire
    public class MissilePowerUpSubscriber : MonoBehaviour
    {
        private void OnEnable()
        {
            EventManager.Subscribe<PowerUpExpiredEvent>(OnPowerUpExpired);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<PowerUpExpiredEvent>(OnPowerUpExpired);
        }

        private void OnPowerUpExpired(PowerUpExpiredEvent e)
        {
            if (e.Definition is IMissileSpawnApplier applier)
            {
                applier.RemoveFromMissile(gameObject);
            }
        }
    }
}
