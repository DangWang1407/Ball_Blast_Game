using Game.Events;
using UnityEngine;

namespace Game.Controllers
{
    public class MissileCollision : MonoBehaviour
    {
        private MissileController missileController;
        private MissileMovement missileMovement;
        private MissilePooling missilePooling;

        private int bounceLeft = 0;
        private bool canBounce = false;

        public void Initialize(MissileController missileController)
        {
            this.missileController = missileController;
            missileMovement = GetComponent<MissileMovement>();
            missilePooling = GetComponent<MissilePooling>();

            bounceLeft = WeaponStats.bounceShot ? 3 : 0;
        }

        public void Update()
        {
            canBounce = WeaponStats.bounceShot;
        }

        public void OnSpawned()
        {
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
        }

        private void HandleWallCollision(Collider2D collision)
        {
            if (canBounce && bounceLeft > 0)
            {
                bounceLeft--;
                missileMovement.Reflect(collision.transform.right);
            }
            else
            {
                missilePooling.DestroyMissile();
            }
        }

        private void HandleGroundCollision(Collider2D collision)
        {
            if(canBounce && bounceLeft > 0)
            {
                bounceLeft--;
                missileMovement.Reflect(collision.transform.up);
            }
            else
            {
                missilePooling.DestroyMissile();
            }
        }

        private void HandleMeteorCollision(Collider2D collision)
        {
            if(!WeaponStats.pierce)
            {
                missilePooling.DestroyMissile();
            }
        }
    }
}