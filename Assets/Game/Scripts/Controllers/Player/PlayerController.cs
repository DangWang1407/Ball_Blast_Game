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