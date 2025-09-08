using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Views
{
    public class BackUI : MonoBehaviour
    {
        Button backButton;
        private void Awake()
        {
            backButton = GetComponent<Button>();
            backButton.onClick.AddListener(OnBackButtonPressed);
        }
        public void OnBackButtonPressed()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            // close level editor window 
#if UNITY_EDITOR
            CloseLevelEditorWindowIfOpen();
#endif
        }

#if UNITY_EDITOR
        private static void CloseLevelEditorWindowIfOpen()
        {
            var windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
            foreach (var w in windows)
            {
                if (w != null && w.titleContent != null && w.titleContent.text == "Level Editor")
                {
                    w.Close();
                    break;
                }
            }
        }
#endif
    }
}
