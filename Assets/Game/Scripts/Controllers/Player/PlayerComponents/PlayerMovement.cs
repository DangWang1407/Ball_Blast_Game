using TMPro;
using UnityEngine;

namespace Game.Controllers
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float smoothing = 0.1f;
        [SerializeField] private HingeJoint2D[] wheels;

        private PlayerController playerController;
        private PlayerBounds playerBounds;
        private JointMotor2D motor;

        private bool isMoving;
        private Vector2 targetPosition;
        private Vector2 currentVelocity;

        public Vector2 TargetPosition { 
            get => targetPosition; 
            set
            {
                targetPosition = value;
                isMoving = true;
            } 
        }

        public void Initialize(PlayerController playerController)
        {
            this.playerController = playerController;
            targetPosition = playerController.Rigidbody.position;
            playerBounds = GetComponent<PlayerBounds>();
            

            if (wheels.Length > 0)
                motor = wheels[0].motor;
        }

        public void FixedUpdate()
        {
            HandleMovement();
            HandleWheels();
        }

        private void HandleMovement()
        {
            if (!isMoving) {
                playerController.Rigidbody.velocity = Vector2.zero;
            }

            if (playerBounds != null) 
            { 
                targetPosition = playerBounds.ClampPosition(targetPosition);
            }

            Vector2 newPosition = Vector2.SmoothDamp(
                playerController.Rigidbody.position,
                targetPosition,
                ref currentVelocity,
                smoothing
            );

            playerController.Rigidbody.MovePosition( newPosition );

            if(Vector2.Distance(playerController.Rigidbody.position, targetPosition) < 0.1)
            {
                isMoving = false;
            }
        }

        private void HandleWheels()
        {
            if (!isMoving) return;
            if (wheels.Length > 0)
            {
                float velocityX = currentVelocity.x;
                if(Mathf.Abs(velocityX) > 0.1f)
                {
                    motor.motorSpeed = velocityX * 150f;
                    ActivateMotor(true);
                }
                else
                {
                    motor.motorSpeed = 0f;
                    ActivateMotor(false);
                }
            }
        }

        private void ActivateMotor(bool isActive)
        {
            foreach (var wheel in wheels)
            {
                wheel.useMotor = isActive;
                wheel.motor = motor;
            }
        }
    }
}