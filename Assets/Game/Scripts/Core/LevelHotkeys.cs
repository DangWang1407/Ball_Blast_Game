using UnityEngine;

namespace Game.Core
{
    public class LevelHotkeys : MonoBehaviour
    {
        [SerializeField] private KeyCode restartLevelKey = KeyCode.R;

        private void Update()
        {
            if (LevelManager.Instance == null) return;

            // Shift + R = reset progress to level 1 and restart
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                && Input.GetKeyDown(restartLevelKey))
            {
                LevelManager.Instance.ResetProgressAndRestart();
                return;
            }

            // R = restart current level
            if (Input.GetKeyDown(restartLevelKey))
            {
                LevelManager.Instance.RestartLevel();
            }
        }
    }
}

