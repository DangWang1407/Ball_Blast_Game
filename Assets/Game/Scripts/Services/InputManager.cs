using UnityEngine;
using Game.Events;
using Game.Core;
using System;

namespace Game.Services
{
    public class InputManager : MonoBehaviour
    {
        private Camera mainCamera;

        [SerializeField] private bool enableInput = true;

        public bool IsPressed { get; private set; }
        public Vector2 MouseWorldPosition { get; private set; }
        public Vector2 TouchPosition { get; private set; }

        public System.Action<Vector2> OnTouchStart;
        public System.Action<Vector2> OnTouchMove;
        public System.Action OnTouchEnd;

        #region Singleton
        public static InputManager Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Initialize()
        {
            EventManager.Subscribe<GameStateChangeEvent>(OnGameStateChanged);
        }
        private void OnGameStateChanged(GameStateChangeEvent gameStateChangeEvent)
        {
            if (gameStateChangeEvent.NewState == GameState.Playing)
            {
                // Enable input handling
                enabled = true;
            }
            else
            {
                // Disable input handling
                enabled = false;
            }
        }
        #endregion


        private void Start()
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main camera not found. InputManager requires a main camera to function.");
                enabled = false; // Disable this script if no camera is found
            }
        }

        private void Update()
        {
            if (!enableInput) return;

            HandleInput();
        }

        void HandleInput()
        {
            bool wasPressed = IsPressed;
            IsPressed = Input.GetMouseButtonDown(0);

            if (mainCamera == null) return;

            Vector3 mousePos = Input.mousePosition;
            MouseWorldPosition = mainCamera.ScreenToWorldPoint(mousePos);
            TouchPosition = MouseWorldPosition;

            if(IsPressed && !wasPressed)
            {
                // Touch/click started
                OnTouchStart?.Invoke(TouchPosition);
                EventManager.Trigger(new PlayerInputEvent(InputType.TouchStart, TouchPosition));
            }

            if(IsPressed && wasPressed)
            {
                // Touch/click held
                OnTouchMove?.Invoke(TouchPosition);
                EventManager.Trigger(new PlayerInputEvent(InputType.TouchMove, TouchPosition));
            }

            if (!IsPressed && wasPressed)
            {
                // Touch/click ended
                OnTouchEnd?.Invoke();
                EventManager.Trigger(new PlayerInputEvent(InputType.TouchEnd, TouchPosition));
            }
        }

        void OnDestroy()
        {
            // Unsubscribe from events to prevent memory leaks
            EventManager.Unsubscribe<GameStateChangeEvent>(OnGameStateChanged);
        }
    }
}