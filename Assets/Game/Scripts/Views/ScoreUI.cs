using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Game.Events;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private void Start()
    {
        EventManager.Subscribe<ScoreUpdateEvent>(OnScoreUpdate);
        UpdateScoreUI(0); // Initialize score UI to 0 at start
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe<ScoreUpdateEvent>(OnScoreUpdate);
    }

    private void OnScoreUpdate(ScoreUpdateEvent scoreEvent)
    {
        Debug.Log($"Score Updated: {scoreEvent.ScoreChange} | New Total: {scoreEvent.newTotalScore} | Reason: {scoreEvent.Reason}");
        UpdateScoreUI(scoreEvent.newTotalScore);
    }

    private void UpdateScoreUI(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
        else
        {
            Debug.LogWarning("Score Text is not assigned in ScoreUI.");
        }
    }
}
