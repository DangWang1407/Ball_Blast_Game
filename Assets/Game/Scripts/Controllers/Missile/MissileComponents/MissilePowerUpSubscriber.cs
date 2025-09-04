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
            EventManager.Subscribe<PowerUpV2ExpiredEvent>(OnPowerUpExpired);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<PowerUpV2ExpiredEvent>(OnPowerUpExpired);
        }

        private void OnPowerUpExpired(PowerUpV2ExpiredEvent e)
        {
            if (e.Definition is IMissileSpawnApplier applier)
            {
                applier.RemoveFromMissile(gameObject);
            }
        }
    }
}
