using UnityEngine;
using Game.Core;
using Game.Services;
using Game.Events;
using System.Collections;

namespace Game.Controllers
{
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private float boundaryOffset = 0.56f; // Offset for screen bounds
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float smoothing = 0.1f;

        [SerializeField] private HingeJoint2D[] wheels;
        [SerializeField] private GameObject shieldPrefabs;
        
        private GameObject shield;

        private bool isShieldActive = false;

        public bool IsInvisible = false;

        private JointMotor2D motor;

        private Rigidbody2D rb;
        private Camera mainCamera;

        private float screenBounds;
        private Vector2 targetPosition;
        private Vector2 currentVelocity;
        private Vector2 lastPosition;

        private bool isMoving;
        private bool isPaused = false;


        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            mainCamera = Camera.main;
        }

        private void Start()
        {
            Initialize();
            CreateShield();
            SubscribeToEvents();

            if(wheels.Length > 0)
            {
                motor = wheels[0].motor;
            }
        }

        private void Update()
        {
            if (isPaused) return;
            HandleLegacyInput();
        }

        private void FixedUpdate()
        {
            if (isPaused) return;
            HandleMovement();
        }

        private void OnDestroy()
        {
            UnsubcribeToEvents();
        }

        private void Initialize()
        {
            if (mainCamera != null)
            {
                screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x - boundaryOffset;
            }
            else
            {
                screenBounds = GameManager.Instance.ScreenWidth - boundaryOffset;
            }

            targetPosition = transform.position;
            Debug.Log("PlayerController initialized with screen bounds: " + screenBounds);
        }

        private void CreateShield()
        {
            shield = Object.Instantiate(shieldPrefabs);
            shield.transform.SetParent(transform);
            shield.transform.localPosition = Vector3.zero;
            shield.tag = "Shield";

            //shield.AddComponent<SpriteRenderer>().sprite = spriteShield.sprite;

            //CircleCollider2D circleCol = shield.AddComponent<CircleCollider2D>();
            //circleCol.radius = 1.5f; 
            //circleCol.isTrigger = true;

            shield.SetActive(false); 
        }

        private void SubscribeToEvents()
        {
            EventManager.Subscribe<PlayerInputEvent>(OnPlayerInput);
            EventManager.Subscribe<GameStateChangeEvent>(OnGameStateChanged);
            EventManager.Subscribe<PowerUpCollectedEvent>(OnPowerUpCollected);
        }

        private void UnsubcribeToEvents()
        {
            EventManager.Unsubscribe<PlayerInputEvent>(OnPlayerInput);
            EventManager.Unsubscribe<GameStateChangeEvent>(OnGameStateChanged);
            EventManager.Unsubscribe<PowerUpCollectedEvent>(OnPowerUpCollected);
        }

        private void HandleLegacyInput()
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                SetTargetPosition(new Vector2(mouseWorldPos.x, mouseWorldPos.y));
            }
        }

        private void HandleMovement()
        {
            if (!isMoving)
            {
                rb.velocity = Vector2.zero;
                return;
            }

            targetPosition.x = Mathf.Clamp(targetPosition.x, -screenBounds, screenBounds);
            targetPosition.y = transform.position.y; // Keep the y position constant

            Vector2 newPosition = Vector2.SmoothDamp(
                rb.position,
                targetPosition,
                ref currentVelocity,
                smoothing
            );

            rb.MovePosition(newPosition);

            if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
            {
                isMoving = false; // Stop moving when close to target
            }

            if (wheels.Length > 0)
            {
                float velocityX = (rb.position.x - lastPosition.x) / Time.fixedDeltaTime;

                if (Mathf.Abs(velocityX) > 0.01f)
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

            lastPosition = rb.position;


        }

        private void ActivateMotor(bool isActive)
        {
            foreach (var wheel in wheels)
            {
                wheel.useMotor = isActive;
                wheel.motor = motor;
            }
        }

        #region Event Handlers
        private void OnPlayerInput(PlayerInputEvent inputEvent)
        {
            switch (inputEvent.Type)
            {
                case InputType.TouchStart:
                case InputType.TouchMove:
                    SetTargetPosition(inputEvent.Position);
                    break;
            }
        }

        private void OnGameStateChanged(GameStateChangeEvent eventData)
        {
            isPaused = eventData.NewState == GameState.Paused;
            if (isPaused)
            {
                rb.velocity = Vector2.zero; // Stop movement when paused
                isMoving = false;
            }
        }

        private void OnPowerUpCollected(PowerUpCollectedEvent eventData)
        {
            if(eventData.PowerUpType == PowerUpType.Invisible)
            {
                               // Handle invisible power-up logic here
                Debug.Log("Invisible power-up collected!");
                // Example: Make player invisible for a duration
                StartCoroutine(InvisiblePowerUpCoroutine(eventData.Duration));
            }
            if(eventData.PowerUpType == PowerUpType.Shield)
            {
                Debug.Log("Shield power-up collected!");
                StartCoroutine(ActivateShield(eventData.Duration));
            }
        }

        IEnumerator InvisiblePowerUpCoroutine(float duration)
        {
            IsInvisible = true;
            var spriteRenderer = GetComponent<SpriteRenderer>();
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f); 
            yield return new WaitForSeconds(duration);
            spriteRenderer.color = originalColor; 
            IsInvisible = false;
        }

        IEnumerator ActivateShield(float duration)
        {
            shield.SetActive(true);
            yield return new WaitForSeconds(duration);
            shield.SetActive(false);
        }
        #endregion


        public void SetTargetPosition(Vector2 position)
        {
            targetPosition = position;
            isMoving = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Meteor"))
            {
                Debug.Log("Meteor hit player - player");
                
            }
        }

    }
}