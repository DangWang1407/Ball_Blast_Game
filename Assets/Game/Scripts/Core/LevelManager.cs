using UnityEngine;
using Game.Events;
using Game.Core;
using System.IO;
using System.IO.Enumeration;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelSaveData
{
    public int currentLevel = 0;
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TextAsset[] levelJsonFiles;
    private int currentLevel = 0;
    private bool levelCompleted = false;
    //private string SavePath => Path.Combine(Application.persistentDataPath, "level_progress.json");
    private string fileName = "level_progress.json";

    public static LevelManager Instance { get; private set; }
    public int CurrentLevel => currentLevel;
    public TextAsset GetCurrentLevelData() => currentLevel < levelJsonFiles.Length ? levelJsonFiles[currentLevel] : null;

    void Awake()
    {
        Instance = this;
        LoadData();
    }

    void Start()
    {
        EventManager.Subscribe<AllMeteorsDestroyedEvent>(OnAllMeteorsDestroyed);
        StartCurrentLevel();
    }

    void OnDestroy()
    {
        EventManager.Unsubscribe<AllMeteorsDestroyedEvent>(OnAllMeteorsDestroyed);
        SaveData();
    }

    void SaveData()
    {
        var saveData = new LevelSaveData { currentLevel = currentLevel};
        SaveManager.SaveData(saveData, fileName);
    }

    void LoadData()
    {
        if (SaveManager.FileExists(fileName))
        {
            var saveData = SaveManager.LoadData<LevelSaveData>(fileName, new LevelSaveData());
            // Restore current level from saved data
            currentLevel = Mathf.Max(0, saveData.currentLevel);
        }
        else SaveData();
    }

    public void StartCurrentLevel()
    {
        levelCompleted = false;
        EventManager.Trigger(new LevelStartEvent(currentLevel, GetCurrentLevelData()));
    }

    // Restart the current level (reload scene)
    public void RestartLevel()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }

    // Reset saved progress to level 0 and restart
    public void ResetProgressAndRestart()
    {
        currentLevel = 0;
        SaveData();
        RestartLevel();
    }

    void OnAllMeteorsDestroyed(AllMeteorsDestroyedEvent evt)
    {
        if (!levelCompleted)
        {
            levelCompleted = true;
            CompleteLevel();
        }
    }

    void CompleteLevel()
    {
        currentLevel++;
        SaveData();

        if (currentLevel >= levelJsonFiles.Length)
            Debug.Log("Game completed!");
        else
            StartCurrentLevel();
    }
}
