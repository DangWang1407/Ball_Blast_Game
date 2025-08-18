using Game.Controllers;
using Game.Events;

namespace Game.PowerUps
{
    public interface IPowerUpWeapon
    {
        PowerUpType Type { get; }
        void Apply(WeaponController controller, float duration);
    }
}

