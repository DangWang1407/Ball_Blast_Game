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
        //private PlayerShield playerShield;
        //private PlayerVisuals playerVisuals;
        private PlayerStats playerStats;

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
            //playerShield = GetComponent<PlayerShield>();
            //playerVisuals = GetComponent<PlayerVisuals>();
            playerStats = GetComponent<PlayerStats>();

            playerMovement.Initialize(this);
            playerBounds.Initialize(this);
            playerInput.Initialize(this);
            //playerShield.Initialize(this);
            //playerVisuals.Initialize(this);
            playerStats.Initialize(this);
        }

        private void Update()
        {
            playerInput.OnUpdate();
        }

        private void FixedUpdate()
        {
            playerMovement.OnFixedUpdate();
        }

        private void OnDestroy()
        {
            playerInput.OnDestroyPlayerInput();
        }
    }
}