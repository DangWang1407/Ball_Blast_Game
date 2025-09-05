using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core
{
    public class InitialSceneLoader : MonoBehaviour
    {
        [SerializeField] private string nextSceneName = "MainMenu";
        [SerializeField] private float delaySeconds = 0f;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            StartCoroutine(LoadNext());
        }

        private System.Collections.IEnumerator LoadNext()
        {
            if (delaySeconds > 0f)
                yield return new WaitForSeconds(delaySeconds);

            if (!string.IsNullOrEmpty(nextSceneName))
            {
                var op = SceneManager.LoadSceneAsync(nextSceneName);
                if (op != null)
                    yield return op;
            }

            Destroy(gameObject);
        }
    }
}

