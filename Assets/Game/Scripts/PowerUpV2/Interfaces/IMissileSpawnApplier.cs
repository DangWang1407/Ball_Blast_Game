using UnityEngine;

namespace Game.PowerUpV2
{
    public interface IMissileSpawnApplier
    {
        void ApplyToMissile(GameObject missile, int level);
        void RemoveFromMissile(GameObject missile);
    }
}
