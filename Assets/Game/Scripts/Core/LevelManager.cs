using UnityEngine;
using Game.Events;
using Game.Core;

public class LevelManager : MonoBehaviour
{
    [Header("Level Data")]
    [SerializeField] private TextAsset[] levelJsonFiles;

    private int currentLevel = 0;
    private bool levelCompleted = false;

    public static LevelManager Instance { get; private set; }

    public int CurrentLevel => currentLevel;
    public int TotalLevels => levelJsonFiles.Length;
    public TextAsset GetCurrentLevelData() => currentLevel < levelJsonFiles.Length ? levelJsonFiles[currentLevel] : null;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        EventManager.Subscribe<AllMeteorsDestroyedEvent>(OnAllMeteorsDestroyed);
        StartCurrentLevel();
    }

    void OnDestroy()
    {
        EventManager.Unsubscribe<AllMeteorsDestroyedEvent>(OnAllMeteorsDestroyed);
    }

    public void StartCurrentLevel()
    {
        levelCompleted = false;
        Debug.Log($"Starting Level {currentLevel + 1}");

        EventManager.Trigger(new LevelStartEvent(currentLevel, GetCurrentLevelData()));
    }

    private void OnAllMeteorsDestroyed(AllMeteorsDestroyedEvent evt)
    {
        if (!levelCompleted)
        {
            levelCompleted = true;
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        Debug.Log($"Level {currentLevel + 1} completed!");

        currentLevel++;

        if (currentLevel >= levelJsonFiles.Length)
        {
            Debug.Log("All levels completed! Game finished!");
            //EventManager.Trigger(new GameCompleteEvent());
        }
        else
        {
            // Start next level
            StartCurrentLevel();
        }
    }

    public void RestartCurrentLevel()
    {
        StartCurrentLevel();
    }

    public void GoToLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelJsonFiles.Length)
        {
            currentLevel = levelIndex;
            StartCurrentLevel();
        }
    }
}