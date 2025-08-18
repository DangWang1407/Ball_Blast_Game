using Game.Controllers;
using Game.Events;

namespace Game.PowerUps
{
    public interface IPowerUpDefend
    {
        PowerUpType Type { get; }
        void Apply(PlayerController controller, float duration);
    }
}

