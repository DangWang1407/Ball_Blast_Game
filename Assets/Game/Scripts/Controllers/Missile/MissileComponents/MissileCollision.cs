using System.Collections.Generic;
using Game.Events;
using UnityEngine;

namespace Game.Controllers
{
    public class MissileCollision : MonoBehaviour
    {
        private MissileController missileController;
        private MissileMovement missileMovement;
        private MissilePooling missilePooling;
        private MissileStats missileStats;

        private List<IMissileCollisionBehavior> collisionBehaviors = new();

        public void Initialize(MissileController missileController)
        {
            this.missileController = missileController;
            missileMovement = GetComponent<MissileMovement>();
            missilePooling = GetComponent<MissilePooling>();
            missileStats = GetComponent<MissileStats>();
        }

        public void AddBehavior(IMissileCollisionBehavior behavior)
        {
            if (!collisionBehaviors.Contains(behavior))
            {
                collisionBehaviors.Add(behavior);
            }
        }

        public void RemoveBehavior(IMissileCollisionBehavior behavior)
        {
            collisionBehaviors.Remove(behavior);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!missileController.IsActive) return;

            if (collision.CompareTag("Wall"))
            {
                HandleWallCollision(collision);
            }

            if (collision.CompareTag("Ground") || collision.CompareTag("UpBounce"))
            {
                HandleGroundCollision(collision);
            }

            if (collision.CompareTag("Meteor"))
            {
                HandleMeteorCollision(collision);
            }
        }

        private void HandleWallCollision(Collider2D collision)
        {
            // Cho behaviors xử lý trước
            bool handled = false;
            foreach (var behavior in collisionBehaviors)
            {
                if (behavior.HandleWallCollision(collision, missileMovement))
                {
                    handled = true;
                    break; 
                }
            }

            // Default behavior
            if (!handled)
            {
                missilePooling.DestroyMissile();
            }
        }

        private void HandleGroundCollision(Collider2D collision)
        {
            bool handled = false;
            foreach (var behavior in collisionBehaviors)
            {
                if (behavior.HandleGroundCollision(collision, missileMovement))
                {
                    handled = true;
                    break;
                }
            }

            if (!handled)
            {
                missilePooling.DestroyMissile();
            }
        }

        private void HandleMeteorCollision(Collider2D collision)
        {
            bool handled = false;
            foreach (var behavior in collisionBehaviors)
            {
                if (behavior.HandleMeteorCollision(collision))
                {
                    handled = true;
                    break;
                }
            }

            if (!handled)
            {
                 missilePooling.DestroyMissile();
            }
        }
    }
}