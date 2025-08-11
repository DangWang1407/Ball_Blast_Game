using Game.Controllers;
using Game.Services;
using System.Collections;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] private string METEOR_POOL = "MeteorPool"; // tên pool
    [SerializeField] private GameObject[] meteorPrefabs;
    [SerializeField] private int initialCount = 10;
    [SerializeField] private float spawnDelay = 1f;

    private void Start()
    {
        // Tạo sẵn các meteors trong pool
        foreach (var prefab in meteorPrefabs)
        {
            PoolMangager.Instance.CreatePool(METEOR_POOL, prefab, initialCount, 50, autoExpand: true);
        }

        StartCoroutine(SpawnMeteors());
    }

    private IEnumerator SpawnMeteors()
    {
        while (true)
        {
            // Spawn random meteor
            GameObject meteor = PoolMangager.Instance.Spawn(METEOR_POOL, transform.position);
            if (meteor != null)
            {
                meteor.GetComponent<MeteorController>()?.Initialize(METEOR_POOL);
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
