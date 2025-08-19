using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Core;
using Game.Events;

namespace Game.Views
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] private Button pauseButton;
        [SerializeField] private TMP_Text buttonText;

        void Start()
        {
            // Setup button
            if (pauseButton != null)
            {
                pauseButton.onClick.AddListener(TogglePause);
            }

            // Get text component if not assigned
            if (buttonText == null && pauseButton != null)
            {
                buttonText = pauseButton.GetComponentInChildren<TMP_Text>();
            }

            // Subscribe to game state changes
            EventManager.Subscribe<GameStateChangeEvent>(OnGameStateChanged);

            UpdateButtonText();
        }

        void OnDestroy()
        {
            EventManager.Unsubscribe<GameStateChangeEvent>(OnGameStateChanged);
        }

        public void TogglePause()
        {
            Debug.Log("Toggle pause");
            if (GameManager.Instance == null) return;

            GameState currentState = GameManager.Instance.CurrentState;

            if (currentState == GameState.Playing)
            {
                GameManager.Instance.PauseGame();
            }
            else if (currentState == GameState.Paused)
            {
                GameManager.Instance.ResumeGame();
            }
        }

        private void OnGameStateChanged(GameStateChangeEvent eventData)
        {
            // Chỉ ẩn pause button khi game over
            if (eventData.NewState == GameState.GameOver || eventData.NewState == GameState.Menu)
            {
                pauseButton.gameObject.SetActive(false);
            }
            else
            {
                pauseButton.gameObject.SetActive(true);
                UpdateButtonText();
            }
        }

        private void UpdateButtonText()
        {
            if (buttonText == null || GameManager.Instance == null) return;

            GameState state = GameManager.Instance.CurrentState;
            buttonText.text = state == GameState.Playing ? "Pause" : "Resume";
        }
    }
}