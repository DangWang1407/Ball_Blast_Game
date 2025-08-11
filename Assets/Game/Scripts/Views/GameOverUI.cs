using Game.Core;
using Game.Events;
using Game.Services;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Views
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TMP_Text finalScoreText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button menuButton;

        void Start()
        {
            // Hide panel initially
            gameOverPanel.SetActive(false);

            // Setup buttons
            restartButton.onClick.AddListener(RestartGame);
            menuButton.onClick.AddListener(GoToMenu);

            // Subscribe to events
            EventManager.Subscribe<GameStateChangeEvent>(OnGameStateChanged);
        }

        void OnDestroy()
        {
            EventManager.Unsubscribe<GameStateChangeEvent>(OnGameStateChanged);
        }

        private void OnGameStateChanged(GameStateChangeEvent eventData)
        {
            if (eventData.NewState == GameState.GameOver)
            {
                ShowGameOver();
            }
            else
            {
                gameOverPanel.SetActive(false);
            }
        }

        private void ShowGameOver()
        {
            gameOverPanel.SetActive(true);

            // Display final score
            if (finalScoreText != null && GameManager.Instance != null)
            {
                finalScoreText.text = $"Final Score: {GameManager.Instance.Score}";
            }

            // Pause time
            Time.timeScale = 0f;
        }

        private void RestartGame()
        {
            Time.timeScale = 1f;
            // Destroy singletons trước khi restart
            if (PoolManager.Instance != null)
                Destroy(PoolManager.Instance.gameObject);
            if (GameManager.Instance != null)
                Destroy(GameManager.Instance.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void GoToMenu()
        {
            Time.timeScale = 1f;
            // Load menu scene or restart
            //SceneManager.LoadScene(0); 
        }
    }
}