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
        EventManager.Subscribe<TotalScoreUpdateEvent>(OnTotalScoreUpdate);
        UpdateScoreUI(0); // Initialize score UI to 0 at start
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe<TotalScoreUpdateEvent>(OnTotalScoreUpdate);
    }

    private void OnTotalScoreUpdate(TotalScoreUpdateEvent scoreEvent)
    {
        UpdateScoreUI(scoreEvent.NewTotalScore);
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
