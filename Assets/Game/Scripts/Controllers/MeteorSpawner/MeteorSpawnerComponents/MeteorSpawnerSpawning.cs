using System.Collections;
using UnityEngine;
using Game.Events;
using Game.Services;

namespace Game.Controllers
{
    public class MeteorSpawnerSpawning : MonoBehaviour
    {
        private MeteorSpawnerController controller;
        private MeteorSpawnerData spawnerData;
        private MeteorSpawnerPooling spawnerPooling;
        private readonly float[] directions = { -1f, 1f };

        public void Initialize(MeteorSpawnerController controller)
        {
            this.controller = controller;
            spawnerData = GetComponent<MeteorSpawnerData>();
            spawnerPooling = GetComponent<MeteorSpawnerPooling>();
        }

        public void StartSpawning()
        {
            StartCoroutine(SpawnMeteorsFromJson());
        }

        private IEnumerator SpawnMeteorsFromJson()
        {
            if (spawnerData.Meteors == null) yield break;

            float startTime = Time.time;
            int currentIndex = 0;

            while (currentIndex < spawnerData.Meteors.meteors.Length)
            {
                var meteorData = spawnerData.Meteors.meteors[currentIndex];
                if (Time.time > startTime + meteorData.spawnTime)
                {
                    SpawnMeteorFromData(meteorData);
                    currentIndex++;
                }
                yield return null;
            }

            EventManager.Trigger(new AllMeteorsDestroyedEvent(LevelManager.Instance.CurrentLevel));
        }

        private void SpawnMeteorFromData(MeteorData meteorData)
        {
            float direction = directions[Random.Range(0, 2)];
            int sizeIndex = (int)meteorData.size;
            Vector3 spawnPosition = (Vector3)meteorData.position;

            GameObject meteor = spawnerPooling.SpawnMeteor(sizeIndex, spawnPosition);

            if (meteor != null)
            {
                var meteorController = meteor.GetComponent<MeteorController>();
                meteorController.Initialize(spawnerPooling.PoolNames[sizeIndex]);
                meteorController.SetMeteorSize(meteorData.size);

                var rb = meteor.GetComponent<Rigidbody2D>();
                Vector2 targetDirection = new Vector2(-(Vector2.zero - (Vector2)spawnPosition).normalized.x, 0);
                rb.velocity = targetDirection * 2f;
                rb.gravityScale = 0f;

                var col = rb.GetComponent<Collider2D>();

                StartCoroutine(EnableGravity(rb, col, 2f, direction));
            }
        }

        private IEnumerator EnableGravity(Rigidbody2D rb, Collider2D col, float delay, float direction)
        {
            yield return new WaitForSeconds(delay);
            if (rb != null)
            {
                rb.gravityScale = 1f;
                rb.AddTorque(Random.Range(-20f, 20f));

                col.isTrigger = false;
                rb.velocity = new Vector2(direction, -0.5f);
            }
        }
    }
}