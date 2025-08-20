using UnityEngine;

namespace Game.Controllers
{
    public class MissileMovement : MonoBehaviour
    {
        private MissileController missileController;

        private Vector2 currentVelocity;

        public void Initialize(MissileController missileController)
        {
            this.missileController = missileController;
        }

        public void OnDespawned()
        {
            missileController.Rigidbody.velocity = Vector2.zero;
            currentVelocity = Vector2.zero;
        }

        public void SetVelocity(Vector2 velocity)
        {
            currentVelocity = velocity * WeaponStats.missileSpeed;
            missileController.Rigidbody.velocity = currentVelocity;

            // Set initial rotation
            if (velocity != Vector2.zero)
            {
                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90;
                missileController.Rigidbody.rotation = angle;
            }
        }

        public void Reflect(Vector2 normal)
        {
            Vector2 reflectedDirection = Vector2.Reflect(currentVelocity.normalized, normal);
            SetVelocity(reflectedDirection);
        }
    }
}