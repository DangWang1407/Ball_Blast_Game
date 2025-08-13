using System.Collections;
using UnityEngine;
using Game.Services;
using Game.Core;
using Game.Events;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] meteorPrefabs; // Large, Medium, Small
    [SerializeField] private int meteorsCount = 12;
    [SerializeField] private float spawnDelay = 4f;

    private readonly string[] poolNames = { "LargeMeteor_Pool", "MediumMeteor_Pool", "SmallMeteor_Pool" };
    private readonly float[] directions = { -1f, 1f };

    public static MeteorSpawner Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CreatePools();
        StartCoroutine(SpawnMeteors());
    }

    void CreatePools()
    {
        for (int i = 0; i < meteorPrefabs.Length; i++)
        {
            PoolManager.Instance.CreatePool(poolNames[i], meteorPrefabs[i], 10, 30, true);
        }
    }

    IEnumerator SpawnMeteors()
    {
        for (int i = 0; i < meteorsCount; i++)
        {
            SpawnMeteor(MeteorSize.Large);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void SpawnMeteor(MeteorSize size)
    {
        int sizeIndex = (int)size;
        float direction = directions[Random.Range(0, 2)];
        float screenOffset = GameManager.Instance.ScreenWidth * 1.3f;

        Vector3 spawnPos = new Vector3(screenOffset * direction, 3f, 0f);
        GameObject meteor = PoolManager.Instance.Spawn(poolNames[sizeIndex], spawnPos);

        if (meteor != null)
        {
            var controller = meteor.GetComponent<MeteorController>();
            controller.Initialize(poolNames[sizeIndex]);
            controller.SetMeteorSize(size);

            var rb = meteor.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(-direction * 2f, 0f);
            rb.gravityScale = 0f;

            StartCoroutine(EnableGravity(rb, 2f));
        }
    }

    IEnumerator EnableGravity(Rigidbody2D rb, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (rb != null)
        {
            rb.gravityScale = 1f;
            rb.AddTorque(Random.Range(-20f, 20f));
        }
    }

    public void SpawnSplitMeteors(Vector3 position, MeteorSize currentSize)
    {
        if (currentSize == MeteorSize.Small) return;

        MeteorSize newSize = currentSize == MeteorSize.Large ? MeteorSize.Medium : MeteorSize.Small;

        for (int i = 0; i < 2; i++)
        {
            GameObject split = PoolManager.Instance.Spawn(poolNames[(int)newSize], position);
            if (split != null)
            {
                split.GetComponent<MeteorController>().Initialize(poolNames[(int)newSize]);
                split.GetComponent<MeteorController>().SetMeteorSize(newSize);
                split.GetComponent<Rigidbody2D>().velocity = new Vector2(directions[i] * 3f, 5f);
            }
        }
    }
}