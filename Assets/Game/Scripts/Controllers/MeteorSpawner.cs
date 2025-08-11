using System.Collections;
using UnityEngine;
using Game.Services;
using Game.Core;
using Game.Events;

public class MeteorSpawner : MonoBehaviour
{
    [Header("Meteor Settings")]
    [SerializeField] private GameObject meteorPrefab; // Chỉ cần 1 prefab
    [SerializeField] private int initialPoolSize = 15;
    [SerializeField] private int maxPoolSize = 50;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnDelay = 2f;
    [SerializeField] private float spawnRangeX = 8f;
    [SerializeField] private float spawnHeight = 6f;
    [SerializeField] private bool autoSpawn = true;

    [Header("Difficulty")]
    [SerializeField] private float minSpawnDelay = 0.5f;
    [SerializeField] private float difficultyIncreaseRate = 0.95f; // Giảm delay theo thời gian
    [SerializeField] private float difficultyIncreaseInterval = 10f; // Mỗi 10s tăng độ khó

    private const string METEOR_POOL = "MeteorPool";
    private Coroutine spawnCoroutine;
    private bool isSpawning = false;

    private void Start()
    {
        InitializePool();
        SubscribeToEvents();

        if (autoSpawn)
        {
            StartSpawning();
        }
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void InitializePool()
    {
        if (meteorPrefab == null)
        {
            Debug.LogError("Meteor prefab is not assigned!");
            return;
        }

        // Tạo pool với size động
        PoolManager.Instance.CreatePool(
            METEOR_POOL,
            meteorPrefab,
            initialPoolSize,
            maxPoolSize,
            autoExpand: true
        );

        Debug.Log($"Meteor pool initialized with {initialPoolSize} objects");
    }

    private void SubscribeToEvents()
    {
        EventManager.Subscribe<GameStateChangeEvent>(OnGameStateChanged);
    }

    private void UnsubscribeFromEvents()
    {
        EventManager.Unsubscribe<GameStateChangeEvent>(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameStateChangeEvent eventData)
    {
        switch (eventData.NewState)
        {
            case GameState.Playing:
                if (autoSpawn && !isSpawning)
                    StartSpawning();
                break;

            case GameState.Paused:
            case GameState.GameOver:
                StopSpawning();
                break;
        }
    }

    public void StartSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }

        spawnCoroutine = StartCoroutine(SpawnMeteorsRoutine());
        isSpawning = true;
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
        isSpawning = false;
    }

    private IEnumerator SpawnMeteorsRoutine()
    {
        float currentSpawnDelay = spawnDelay;
        float timeSinceStart = 0f;

        while (true)
        {
            // Check if game is still playing
            if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameState.Playing)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            // Spawn meteor
            SpawnRandomMeteor();

            // Wait for spawn delay
            yield return new WaitForSeconds(currentSpawnDelay);

            // Increase difficulty over time
            timeSinceStart += currentSpawnDelay;
            if (timeSinceStart >= difficultyIncreaseInterval)
            {
                currentSpawnDelay = Mathf.Max(minSpawnDelay, currentSpawnDelay * difficultyIncreaseRate);
                timeSinceStart = 0f;
                Debug.Log($"Difficulty increased! New spawn delay: {currentSpawnDelay:F2}s");
            }
        }
    }

    private void SpawnRandomMeteor()
    {
        // Random spawn position
        Vector3 spawnPosition = new Vector3(
            Random.Range(-spawnRangeX, spawnRangeX),
            spawnHeight,
            transform.position.z
        );

        // Spawn meteor from pool
        GameObject meteor = PoolManager.Instance.Spawn(METEOR_POOL, spawnPosition);

        if (meteor != null)
        {
            var meteorController = meteor.GetComponent<MeteorController>();
            if (meteorController != null)
            {
                // Always spawn as Large meteor
                meteorController.SetMeteorSize(MeteorSize.Large);
                meteorController.Initialize(METEOR_POOL);

                // Add some random horizontal velocity
                Rigidbody2D meteorRb = meteor.GetComponent<Rigidbody2D>();
                if (meteorRb != null)
                {
                    float randomX = Random.Range(-2f, 2f);
                    meteorRb.velocity = new Vector2(randomX, meteorRb.velocity.y);
                }
            }
        }
        else
        {
            Debug.LogWarning("Failed to spawn meteor from pool!");
        }
    }

    // Public method để spawn meteor manually (for testing)
    public void SpawnMeteorAtPosition(Vector3 position, MeteorSize size = MeteorSize.Large)
    {
        GameObject meteor = PoolManager.Instance.Spawn(METEOR_POOL, position);

        if (meteor != null)
        {
            var meteorController = meteor.GetComponent<MeteorController>();
            meteorController?.SetMeteorSize(size);
            meteorController?.Initialize(METEOR_POOL);
        }
    }

    // Gizmos để visualize spawn area
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(
            new Vector3(0, spawnHeight, 0),
            new Vector3(spawnRangeX * 2, 0.2f, 0)
        );
    }

    // Public properties để tweak từ code khác
    public float CurrentSpawnDelay
    {
        get { return spawnDelay; }
        set { spawnDelay = Mathf.Max(0.1f, value); }
    }

    public bool IsSpawning => isSpawning;
}