using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Views
{
    public class BattlePage : MonoBehaviour
    {
        [SerializeField] private Button playButton;

        private void Start()
        {
            playButton.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            SceneManager.LoadScene("GamePlay");
        }
    }
}