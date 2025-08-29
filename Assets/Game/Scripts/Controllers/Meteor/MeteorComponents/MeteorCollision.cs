using Game.Controllers;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Game.Controllers
{
    public class MeteorCollision : MonoBehaviour
    {
        [SerializeField] private float jumpForce = 12f;
        private MeteorController meteorController;
        private MeteorHealth meteorHealth;

        private Collider2D col;

        private Vector2 lastVelocity;

        public void Initialize(MeteorController meteorController)
        {
            this.meteorController = meteorController;
            meteorHealth = GetComponent<MeteorHealth>();
            col = GetComponent<Collider2D>();
        }

        public void OnFixedUpdate()
        {
            lastVelocity = meteorController.Rigidbody.velocity;
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
                    col.isTrigger = false;
                    //HandleGroundCollision(collision);
                    break;
                case "UpBounce":
                    col.isTrigger = false;
                    //HandleUpBounce(collision);
                    break;
                case "Player":
                    HandlePlayerCollision(collision);
                    break;
                case "Shield":
                    col.isTrigger = false;
                    //HandleShieldCollision(collision);
                    break;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "Missile":
                    //HandleMissileCollision(collision);
                    break;
                case "Wall":
                   
                case "Ground":
                   
                case "UpBounce":
                    ReflectVelocity(collision);
                    break;
                case "Player":
                    break;
                case "Shield":
                    break;
            }
        }

        private void HandleMissileCollision(Collider2D collision)
        {
            meteorHealth.TakeDamage(1);
        }

        private void HandleWallCollision(Collider2D collision)
        {
            float posX = transform.position.x;
            var velocity = meteorController.Rigidbody.velocity;
            meteorController.Rigidbody.velocity = new Vector2(
                posX > 0 ? -Mathf.Abs(velocity.x) : Mathf.Abs(velocity.x),
                velocity.y);
        }

        private void HandleGroundCollision(Collider2D collision)
        {
            meteorController.Rigidbody.velocity = new Vector2(meteorController.Rigidbody.velocity.x, jumpForce);
        }

        private void HandleUpBounce(Collider2D collision)
        {
            meteorController.Rigidbody.velocity = Vector2.Reflect(meteorController.Rigidbody.velocity.normalized, collision.transform.up);
        }

        private void HandlePlayerCollision(Collider2D collision)
        {

        }

        private void HandleShieldCollision(Collider2D collision)
        {
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            meteorController.Rigidbody.velocity = direction * 10f;
        }

        private void ReflectVelocity(Collision2D collision)
        {
            
            var reflectDirection = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
            meteorController.Rigidbody.velocity = reflectDirection * 10f;
        }
    }
}