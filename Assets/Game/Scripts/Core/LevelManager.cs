using UnityEngine;
using Game.Events;
using Game.Core;
using System.IO;

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
    private string SavePath => Path.Combine(Application.persistentDataPath, "level_progress.json");

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
        try
        {
            var saveData = new LevelSaveData { currentLevel = currentLevel};
            File.WriteAllText(SavePath, JsonUtility.ToJson(saveData));
        }
        catch { Debug.LogError("Save failed"); }
    }

    void LoadData()
    {
        try
        {
            if (File.Exists(SavePath))
            {
                var saveData = JsonUtility.FromJson<LevelSaveData>(File.ReadAllText(SavePath));
                currentLevel = Mathf.Clamp(saveData.currentLevel, 0, levelJsonFiles.Length - 1);

                Debug.Log("Level data loaded successfully");
                Debug.Log(currentLevel);
            }
            else SaveData();
        }
        catch { currentLevel = 0; }
    }

    public void StartCurrentLevel()
    {
        levelCompleted = false;
        EventManager.Trigger(new LevelStartEvent(currentLevel, GetCurrentLevelData()));
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