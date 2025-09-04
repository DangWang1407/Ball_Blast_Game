using UnityEngine;

namespace Game.PowerUpV2
{
    public interface IPlayerApplier
    {
        void ApplyToPlayer(GameObject player, int level);
        void RemoveFromPlayer(GameObject player);
    }
}

