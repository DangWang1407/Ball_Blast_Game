using Game.Events;
using UnityEngine;

namespace Game.Controllers
{
    public class MissileCollision : MonoBehaviour
    {
        private MissileController missileController;
        private MissileMovement missileMovement;

        private int bounceLeft = 0;
        private bool canBounce = false;

        public void Initialize(MissileController missileController)
        {
            this.missileController = missileController;
            missileMovement = GetComponent<MissileMovement>();

            bounceLeft = WeaponStats.bounceShot ? 3 : 0;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!missileController.IsActive) return;
            if(collision.CompareTag("Wall"))
            {
                HandleWallCollision(collision);
            }

            if(collision.CompareTag("Ground") || collision.CompareTag("UpBounce"))
            {
                HandleGroundCollision(collision);
            }

            if(collision.CompareTag("Meteor"))
            {
                HandleMeteorCollision(collision);
            }
            //if (other.CompareTag("Wall"))
            //{
            //    if (WeaponStats.bounceShot && bounceLeft > 0)
            //    {
            //        bounceLeft--;
            //        Vector2 reflectDirection = Vector2.Reflect(Rigidbody.velocity.normalized, other.transform.right);
            //        SetVelocity(reflectDirection);
            //    }
            //    else
            //    {
            //        DestroyMissile(MissileDestroyReason.OutOfBounds);
            //    }
            //}
            //if (other.CompareTag("Ground") || other.CompareTag("UpBounce"))
            //{
            //    if (WeaponStats.bounceShot && bounceLeft > 0)
            //    {
            //        bounceLeft--;
            //        Vector2 reflectDirection = Vector2.Reflect(Rigidbody.velocity.normalized, other.transform.up);
            //        SetVelocity(reflectDirection);
            //    }
            //    else
            //    {
            //        DestroyMissile(MissileDestroyReason.OutOfBounds);
            //    }
            //}
            //if (other.CompareTag("Meteor") && !WeaponStats.pierce)
            //{
            //    DestroyMissile(MissileDestroyReason.HitTarget);
            //}
        }

        private void HandleWallCollision(Collider2D collision)
        {

        }

        private void HandleGroundCollision(Collider2D collision)
        {

        }

        private void HandleMeteorCollision(Collider2D collision)
        {

        }
    }
}