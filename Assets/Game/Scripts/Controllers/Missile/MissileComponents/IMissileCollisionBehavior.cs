using UnityEngine;

namespace Game.Controllers
{
    public interface IMissileCollisionBehavior
    {
        bool HandleWallCollision(Collider2D wall, MissileMovement movement);
        bool HandleGroundCollision(Collider2D ground, MissileMovement movement);
        bool HandleMeteorCollision(Collider2D meteor);
    }
}