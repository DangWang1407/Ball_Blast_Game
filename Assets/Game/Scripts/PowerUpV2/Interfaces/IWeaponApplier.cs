using UnityEngine;

namespace Game.PowerUpV2
{
    public interface IWeaponApplier
    {
        void ApplyToWeapon(GameObject weapon, int level);
        void RemoveFromWeapon(GameObject weapon);
    }
}

