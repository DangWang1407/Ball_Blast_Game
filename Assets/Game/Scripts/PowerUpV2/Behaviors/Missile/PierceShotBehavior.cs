using Game.Controllers;
using UnityEngine;

namespace Game.PowerUpV2
{
    public class PierceShotBehavior : MonoBehaviour, IMissileCollisionBehavior
    {
        private MissileCollision missileCollision;

        private void OnEnable()
        {
            missileCollision = GetComponent<MissileCollision>();
            if (missileCollision != null)
            {
                missileCollision.AddBehavior(this);
            }
        }

        private void OnDisable()
        {
            if (missileCollision != null)
            {
                missileCollision.RemoveBehavior(this);
            }
        }

        public bool HandleWallCollision(Collider2D wall, MissileMovement movement)
        {
            return false;
        }

        public bool HandleGroundCollision(Collider2D ground, MissileMovement movement)
        {
            return false;
        }

        public bool HandleMeteorCollision(Collider2D meteor)
        {
            // Pierce through meteor: let missile survive
            return true;
        }
    }
}

