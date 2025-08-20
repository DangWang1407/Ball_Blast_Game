using UnityEngine;

namespace Game.Controllers
{
    public class MeteorCollision : MonoBehaviour
    {
        private MeteorController meteorController;
        private MeteorHealth meteorHealth;

        public void Initialize(MeteorController meteorController)
        {
            this.meteorController = meteorController;
            meteorHealth = GetComponent<MeteorHealth>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            switch (collision.tag)
            {
                case "Missile":
                    HandleMissileCollision(collision);
                    break;
                case "Wall":
                    HandleWallCollision(collision);
                    break;
                case "Ground":
                case "UpBounce":
                    HandleGroundCollision(collision);
                    break;
                case "Player":
                    HandlePlayerCollision(collision);
                    break;
                case "Shield":
                    HandleShieldCollision(collision);
                    break;
            }
        }

        private void HandleMissileCollision(Collider2D collision)
        {
            meteorHealth.TakeDamage(WeaponStats.damage);
        }

        private void HandleWallCollision(Collider2D collision)
        {
            meteorController.Rigidbody.velocity = Vector2.Reflect(meteorController.Rigidbody.velocity, transform.right);
        }

        private void HandleGroundCollision(Collider2D collision)
        {
            meteorController.Rigidbody.velocity = Vector2.Reflect(meteorController.Rigidbody.velocity, transform.up);
        }

        private void HandlePlayerCollision(Collider2D collision)
        {

        }

        private void HandleShieldCollision(Collider2D collision)
        {
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            meteorController.Rigidbody.velocity = direction * 10f;
        }
    }
}