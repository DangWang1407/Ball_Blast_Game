using Game.Events;
using TMPro;
using UnityEngine;

namespace Game.Views
{
    public class LevelUI : MonoBehaviour
    {
        [SerializeField] TMP_Text levelText;

        private void Awake()
        {
            EventManager.Subscribe<LevelStartEvent>(OnLevelStart);
        }

        private void OnLevelStart(LevelStartEvent e)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            levelText.text = $"Level: {LevelManager.Instance.CurrentLevel + 1}";
        }

        private void OnDestroy()
        {
            EventManager.Unsubscribe<LevelStartEvent>(OnLevelStart);
        }
    }
}