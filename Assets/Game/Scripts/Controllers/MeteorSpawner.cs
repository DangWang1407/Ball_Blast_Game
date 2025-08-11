using Game.Controllers;
using Game.Services;
using System.Collections;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] private string METEOR_POOL = "MeteorPool";
    [SerializeField] private GameObject[] meteorPrefabs;
    [SerializeField] private int initialCount = 10;
    [SerializeField] private float spawnDelay = 1f;

    // Thêm các field này
    [SerializeField] private float spawnRangeX = 2f; // Độ rộng spawn area
    [SerializeField] private float spawnHeight = 2f; // Độ cao spawn

    private void Start()
    {
        // Tạo pool cho từng loại meteor riêng biệt
        for (int i = 0; i < meteorPrefabs.Length; i++)
        {
            string poolName = $"{METEOR_POOL}_{i}";
            PoolManager.Instance.CreatePool(poolName, meteorPrefabs[i], initialCount, 50, autoExpand: true);
        }

        StartCoroutine(SpawnMeteors());
    }

    private IEnumerator SpawnMeteors()
    {
        while (true)
        {
            // Random vị trí spawn
            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnRangeX, spawnRangeX), // Random X
                spawnHeight, // Fixed Y (trên cao)
                transform.position.z
            );

            // Random loại meteor
            int randomIndex = Random.Range(0, meteorPrefabs.Length);
            string poolName = $"{METEOR_POOL}_{randomIndex}";

            GameObject meteor = PoolManager.Instance.Spawn(poolName, spawnPosition);
            if (meteor != null)
            {
                meteor.GetComponent<MeteorController>()?.Initialize(poolName);
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}