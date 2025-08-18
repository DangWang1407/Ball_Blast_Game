using UnityEngine;
using Game.Core;
using Game.Services;
using Game.Events;
using Game.PowerUps;
using System.Collections;
using System.Collections.Generic;

namespace Game.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float boundaryOffset = 0.56f;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float smoothing = 0.1f;

        [SerializeField] private HingeJoint2D[] wheels;
        [SerializeField] private GameObject shieldPrefabs;

        private Rigidbody2D rb;
        private Camera mainCamera;
        private JointMotor2D motor;

        private Vector2 targetPosition;
        private Vector2 currentVelocity;
        private Vector2 lastPosition;

        private float screenBounds;
        private bool isMoving;
        private bool isPaused = false;

        public bool IsInvisible = false;
        public GameObject Shield { get; private set; }

        private Dictionary<PowerUpType, IPowerUpDefend> powerUps;

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

            if (wheels.Length > 0)
                motor = wheels[0].motor;

            // Khởi tạo dictionary PowerUps
            powerUps = new Dictionary<PowerUpType, IPowerUpDefend>
            {
                { PowerUpType.Invisible, new InvisiblePowerUp() },
                { PowerUpType.Shield, new ShieldPowerUp() }
            };
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
            UnsubscribeToEvents();
        }

        private void Initialize()
        {
            if (mainCamera != null)
                screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x - boundaryOffset;
            else
                screenBounds = GameManager.Instance.ScreenWidth - boundaryOffset;

            targetPosition = transform.position;
            Debug.Log("PlayerController initialized with screen bounds: " + screenBounds);
        }

        private void CreateShield()
        {
            Shield = Object.Instantiate(shieldPrefabs);
            Shield.transform.SetParent(transform);
            Shield.transform.localPosition = Vector3.zero;
            Shield.tag = "Shield";
            Shield.SetActive(false);
        }

        private void SubscribeToEvents()
        {
            EventManager.Subscribe<PlayerInputEvent>(OnPlayerInput);
            EventManager.Subscribe<GameStateChangeEvent>(OnGameStateChanged);
            EventManager.Subscribe<PowerUpCollectedEvent>(OnPowerUpCollected);
        }

        private void UnsubscribeToEvents()
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
            targetPosition.y = transform.position.y;

            Vector2 newPosition = Vector2.SmoothDamp(
                rb.position,
                targetPosition,
                ref currentVelocity,
                smoothing
            );

            rb.MovePosition(newPosition);

            if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
                isMoving = false;

            if (wheels.Length > 0)
            {
                float velocityX = (rb.position.x - lastPosition.x) / Time.fixedDeltaTime;

                if (Mathf.Abs(velocityX) > 0.1f)
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
                rb.velocity = Vector2.zero;
                isMoving = false;
            }
        }

        private void OnPowerUpCollected(PowerUpCollectedEvent eventData)
        {
            if (powerUps.TryGetValue(eventData.PowerUpType, out var powerUp))
            {
                powerUp.Apply(this, eventData.Duration);
            }
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
