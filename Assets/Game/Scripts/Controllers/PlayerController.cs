using UnityEngine;
using Game.Core;
using Game.Services;
using Game.Events;

namespace Game.Controllers
{
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private float boundaryOffset = 0.56f; // Offset for screen bounds

        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float smoothing = 0.1f;

        private Rigidbody2D rb;
        private Camera mainCamera;

        private float screenBounds;
        private Vector2 targetPosition;
        private Vector2 currentVelocity;

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
            SubscribeToEvents();
        }

        private void Update()
        {
            if (isPaused) return;
            HandleLegacyInput();
        }

        private void FixedUpdate()
        {
            if(isPaused) return;
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

        private void SubscribeToEvents()
        {
            EventManager.Subscribe<PlayerInputEvent>(OnPlayerInput);
            EventManager.Subscribe<GameStateChangeEvent>(OnGameStateChanged);
        }

        private void UnsubcribeToEvents()
        {
            EventManager.Unsubscribe<PlayerInputEvent>(OnPlayerInput);
            EventManager.Unsubscribe<GameStateChangeEvent>(OnGameStateChanged);
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
            if(!isMoving)
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
                // Xử lý giảm máu, chết, v.v.
            }
        }

    }
}
