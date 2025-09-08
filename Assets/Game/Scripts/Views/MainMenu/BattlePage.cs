using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Views
{
    public class BattlePage : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button levelEditorButton;

        private void Start()
        {
            playButton.onClick.AddListener(StartGame);
            levelEditorButton.onClick.AddListener(OpenLevelEditor);
        }

        private void StartGame()
        {
            SceneManager.LoadScene("GamePlay");
        }

        private void OpenLevelEditor()
        {
#if UNITY_EDITOR
            // Open the editor window after the LevelEditor scene has loaded
            SceneManager.sceneLoaded += OnLevelEditorSceneLoaded;
#endif
            SceneManager.LoadScene("LevelEditor");
        }

#if UNITY_EDITOR
        private void OnLevelEditorSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "LevelEditor") return;
            SceneManager.sceneLoaded -= OnLevelEditorSceneLoaded;

            // Trigger the menu item which opens the Level Editor window
            EditorApplication.ExecuteMenuItem("Window/Level Editor");
        }
#endif
    }
}
