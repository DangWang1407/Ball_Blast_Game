using UnityEngine;

namespace Game.Controllers
{
    public class MeteorCollision : MonoBehaviour
    {
        [SerializeField] private float jumpForce = 12f;
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
                    HandleGroundCollision(collision);
                    break;
                case "UpBounce":
                    HandleUpBounce(collision);
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
            meteorHealth.TakeDamage(1);
        }

        private void HandleWallCollision(Collider2D collision)
        {
            // Apply bounce forces only after falling starts
            var rb = meteorController.Rigidbody;
            if (rb.gravityScale < 0.5f) return;

            float posX = transform.position.x;
            if (posX > 0)
            {
                rb.AddForce(Vector2.left * 150f);
            }
            else
            {
                rb.AddForce(Vector2.right * 150f);
            }
            rb.AddTorque(posX * 4f);
        }

        private void HandleGroundCollision(Collider2D collision)
        {
            var rb = meteorController.Rigidbody;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            rb.AddTorque(-rb.angularVelocity * 4f);
        }

        private void HandleUpBounce(Collider2D collision)
        {
            meteorController.Rigidbody.velocity = Vector2.Reflect(meteorController.Rigidbody.velocity.normalized, collision.transform.up);
        }

        private void HandlePlayerCollision(Collider2D collision)
        {
            // player die 
        }

        private void HandleShieldCollision(Collider2D collision)
        {
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            meteorController.Rigidbody.velocity = direction * 10f;
        }
    }
}
