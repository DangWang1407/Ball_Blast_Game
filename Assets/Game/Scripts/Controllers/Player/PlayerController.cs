//using UnityEngine;
//using Game.Core;
//using Game.Services;
//using Game.Events;
//using Game.PowerUps;
//using System.Collections;
//using System.Collections.Generic;

//namespace Game.Controllers
//{
//    public class PlayerController : MonoBehaviour
//    {
//        [SerializeField] private float boundaryOffset = 0.56f;
//        [SerializeField] private float moveSpeed = 5f;
//        [SerializeField] private float smoothing = 0.1f;

//        [SerializeField] private HingeJoint2D[] wheels;
//        [SerializeField] private GameObject shieldPrefabs;

//        public Rigidbody2D Rigidbody { get; private set; }
//        public Camera Camera { get; private set; }
//        private JointMotor2D motor;

//        private Vector2 targetPosition;
//        private Vector2 currentVelocity;
//        private Vector2 lastPosition;

//        private float screenBounds;
//        private bool isMoving;
//        private bool isPaused = false;

//        public bool IsInvisible = false;
//        public GameObject Shield { get; private set; }

//        private void Awake()
//        {
//            Rigidbody = GetComponent<Rigidbody2D>();
//            Camera = Camera.main;
//        }

//        private void Start()
//        {
//            Initialize();
//            CreateShield();
//            SubscribeToEvents();

//            if (wheels.Length > 0)
//                motor = wheels[0].motor;
//        }

//        private void Update()
//        {
//            if (isPaused) return;
//            HandleLegacyInput();
//        }

//        private void FixedUpdate()
//        {
//            if (isPaused) return;
//            HandleMovement();
//        }

//        private void OnDestroy()
//        {
//            UnsubscribeToEvents();
//        }

//        private void Initialize()
//        {
//            if (Camera != null)
//                screenBounds = Camera.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x - boundaryOffset;
//            else
//                screenBounds = GameManager.Instance.ScreenWidth - boundaryOffset;

//            targetPosition = transform.position;
//            Debug.Log("PlayerController initialized with screen bounds: " + screenBounds);
//        }

//        private void CreateShield()
//        {
//            Shield = Object.Instantiate(shieldPrefabs);
//            Shield.transform.SetParent(transform);
//            Shield.transform.localPosition = Vector3.zero;
//            Shield.tag = "Shield";
//            Shield.SetActive(false);
//        }

//        private void SubscribeToEvents()
//        {
//            EventManager.Subscribe<PlayerInputEvent>(OnPlayerInput);
//            EventManager.Subscribe<GameStateChangeEvent>(OnGameStateChanged);
//        }

//        private void UnsubscribeToEvents()
//        {
//            EventManager.Unsubscribe<PlayerInputEvent>(OnPlayerInput);
//            EventManager.Unsubscribe<GameStateChangeEvent>(OnGameStateChanged);
//        }

//        private void HandleLegacyInput()
//        {
//            if (Input.GetMouseButton(0))
//            {
//                Vector3 mouseWorldPos = Camera.ScreenToWorldPoint(Input.mousePosition);
//                SetTargetPosition(new Vector2(mouseWorldPos.x, mouseWorldPos.y));
//            }
//        }

//        private void HandleMovement()
//        {
//            if (!isMoving)
//            {
//                Rigidbody.velocity = Vector2.zero;
//                return;
//            }

//            //need screenbouds component here
//            targetPosition.x = Mathf.Clamp(targetPosition.x, -screenBounds, screenBounds);
//            targetPosition.y = transform.position.y;

//            Vector2 newPosition = Vector2.SmoothDamp(
//                Rigidbody.position,
//                targetPosition,
//                ref currentVelocity,
//                smoothing
//            );

//            Rigidbody.MovePosition(newPosition);

//            if (Vector2.Distance(Rigidbody.position, targetPosition) < 0.01f)
//                isMoving = false;

//            if (wheels.Length > 0)
//            {
//                //float velocityX = (Rigidbody.position.x - lastPosition.x) / Time.fixedDeltaTime;
//                float velocityX = currentVelocity.x;

//                if (Mathf.Abs(velocityX) > 0.1f)
//                {
//                    motor.motorSpeed = velocityX * 150f;
//                    ActivateMotor(true);
//                }
//                else
//                {
//                    motor.motorSpeed = 0f;
//                    ActivateMotor(false);
//                }
//            }

//            lastPosition = Rigidbody.position;
//        }

//        private void ActivateMotor(bool isActive)
//        {
//            foreach (var wheel in wheels)
//            {
//                wheel.useMotor = isActive;
//                wheel.motor = motor;
//            }
//        }

//        #region Event Handlers
//        private void OnPlayerInput(PlayerInputEvent inputEvent)
//        {
//            switch (inputEvent.Type)
//            {
//                case InputType.TouchStart:
//                case InputType.TouchMove:
//                    SetTargetPosition(inputEvent.Position);
//                    break;
//            }
//        }

//        private void OnGameStateChanged(GameStateChangeEvent eventData)
//        {
//            isPaused = eventData.NewState == GameState.Paused;
//            if (isPaused)
//            {
//                Rigidbody.velocity = Vector2.zero;
//                isMoving = false;
//            }
//        }
//        #endregion

//        public void SetTargetPosition(Vector2 position)
//        {
//            targetPosition = position;
//            isMoving = true;
//        }

//        private void OnTriggerEnter2D(Collider2D other)
//        {
//            if (other.CompareTag("Meteor"))
//            {
//                Debug.Log("Meteor hit player - player");
//            }
//        }
//    }
//}


using UnityEngine;

namespace Game.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        public Rigidbody2D Rigidbody { get; private set; }
        public Camera Camera { get; private set; }

        //Components
        private PlayerBounds playerBounds;
        private PlayerInput playerInput;
        private PlayerMovement playerMovement;
        private PlayerShield playerShield;
        private PlayerVisuals playerVisuals;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Camera = Camera.main;

            playerMovement = GetComponent<PlayerMovement>();
            playerInput = GetComponent<PlayerInput>();
            playerBounds = GetComponent<PlayerBounds>();
            playerShield = GetComponent<PlayerShield>();
            playerVisuals = GetComponent<PlayerVisuals>();

            playerMovement.Initialize(this);
            playerBounds.Initialize(this);
            playerInput.Initialize(this);
            playerShield.Initialize(this);
            playerVisuals.Initialize(this);
        }

        private void Update()
        {
            playerInput.Update();
        }

        private void FixedUpdate()
        {
            playerMovement.FixedUpdate();
        }

        private void OnDestroy()
        {
            playerInput.OnDestroy();
        }
    }
}