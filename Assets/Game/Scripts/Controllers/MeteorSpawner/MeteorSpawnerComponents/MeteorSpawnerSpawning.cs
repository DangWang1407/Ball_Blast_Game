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

        private int activeMeteors = 0;
        private bool spawnFinished = false;
        private int startedLevelIndex = 0; // track level index from LevelStartEvent

        public void Initialize(MeteorSpawnerController controller)
        {
            this.controller = controller;
            spawnerData = GetComponent<MeteorSpawnerData>();
            spawnerPooling = GetComponent<MeteorSpawnerPooling>();
            // Subscribe to relevant events
            EventManager.Subscribe<MeteorDestroyedEvent>(OnMeteorDestroyed);
            EventManager.Subscribe<LevelStartEvent>(OnLevelStart);
            EventManager.Subscribe<SplitMeteorEvent>(OnSplitMeteor);
        }

        public void StartSpawning()
        {
            // Reset counters at the start of a spawn sequence
            activeMeteors = 0;
            spawnFinished = false;
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
            // Mark spawning as finished; completion will be checked when meteors are cleared
            spawnFinished = true;
            TryCompleteLevel();
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
                if (meteorData.health > 0)
                {
                    meteorController.SetMeteorHealth(meteorData.health);
                }

                // Track active meteors for completion logic
                activeMeteors++;

                var rb = meteor.GetComponent<Rigidbody2D>();

                // Spawn off-screen horizontally, drift in, then fall
                float worldHalfWidth = 5f;
                if (Camera.main != null)
                {
                    worldHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
                }
                float screenOffset = worldHalfWidth * 1.3f;

                // Keep Y/Z from level, push X to off-screen side
                Vector3 offscreenPos = spawnPosition;
                offscreenPos.x = screenOffset * direction;
                meteor.transform.position = offscreenPos;

                float horizontalSpeed = 1f;
                rb.gravityScale = 0f;
                rb.velocity = new Vector2(-direction * horizontalSpeed, 0f);

                float timeToCenter = screenOffset / Mathf.Max(0.01f, horizontalSpeed);
                float delay = Mathf.Clamp(Random.Range(timeToCenter - 2.5f, timeToCenter - 1f), 0.5f, timeToCenter + 1.5f);
                StartCoroutine(EnableGravity(rb, delay));
            }
        }

        private IEnumerator EnableGravity(Rigidbody2D rb, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (rb != null)
            {
                rb.gravityScale = 1f;
                rb.AddTorque(Random.Range(-20f, 20f));
            }
        }

        private void OnMeteorDestroyed(MeteorDestroyedEvent _)
        {
            if (activeMeteors > 0) activeMeteors--;
            TryCompleteLevel();
        }

        private void OnLevelStart(LevelStartEvent e)
        {
            // Reset on new level
            activeMeteors = 0;
            spawnFinished = false;
            startedLevelIndex = e.LevelIndex;
        }

        private void OnSplitMeteor(SplitMeteorEvent e)
        {
            activeMeteors += 2;
        }

        private void TryCompleteLevel()
        {
            if (spawnFinished && activeMeteors <= 0)
            {
                EventManager.Trigger(new AllMeteorsDestroyedEvent(startedLevelIndex));
            }
        }

        private void OnDestroy()
        {
            EventManager.Unsubscribe<MeteorDestroyedEvent>(OnMeteorDestroyed);
            EventManager.Unsubscribe<LevelStartEvent>(OnLevelStart);
            EventManager.Unsubscribe<SplitMeteorEvent>(OnSplitMeteor);
        }
    }
}
