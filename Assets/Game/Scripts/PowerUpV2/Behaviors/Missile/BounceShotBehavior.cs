using Game.Controllers;
using UnityEngine;
using Game.PowerUp;

namespace Game.PowerUpV2
{
    public class BounceShotBehavior : MonoBehaviour, IMissileCollisionBehavior
    {
        private int bounceLeft;
        private MissileCollision missileCollision;

        public void Init(BounceShotDefinition def, int level)
        {
            bounceLeft = def != null ? def.GetMaxBounces(level) : 3;
        }

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
            if (bounceLeft > 0)
            {
                bounceLeft--;
                movement.Reflect(wall.transform.right);
                return true;
            }
            return false;
        }

        public bool HandleGroundCollision(Collider2D ground, MissileMovement movement)
        {
            if (bounceLeft > 0)
            {
                bounceLeft--;
                movement.Reflect(ground.transform.up);
                return true;
            }
            return false;
        }

        public bool HandleMeteorCollision(Collider2D meteor)
        {
            return false;
        }
    }
}

