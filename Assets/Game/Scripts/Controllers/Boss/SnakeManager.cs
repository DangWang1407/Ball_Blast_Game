using System.Collections.Generic;
using UnityEngine;

namespace Game.Controllers
{
    [System.Serializable]
    public class SnakeBodyData
    {
        public int index;
        public int health;
    }

    [System.Serializable]
    public class SnakeConfig
    {
        public SnakeBodyData[] bodyParts;
    }

    public class SnakeManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject headPrefab; // Fake head (invisible)
        [SerializeField] private GameObject realHeadPrefab; // Real head prefab
        [SerializeField] private GameObject bodyPrefab;

        [Header("Config")]
        [SerializeField] private TextAsset jsonConfig;
        [SerializeField] private float distanceBetween = 0.8f;
        [SerializeField] private float speed = 0.9f;

        [Header("Sine Wave")]
        [SerializeField] private float waveAmplitude = 15f;
        [SerializeField] private float waveFrequency = 0.5f;

        [Header("Collapse Settings")]
        [SerializeField] private bool enableCollapseBackward = true;
        [SerializeField] private float collapseAnimationSpeed = 5f;

        private List<GameObject> snakeBody = new List<GameObject>();
        private List<SnakeBodyData> bodyDataQueue = new List<SnakeBodyData>();
        private List<int> bodyPartMarkerOffsets = new List<int>();
        private List<bool> isAnimatingCollapse = new List<bool>();
        private List<float> collapseAnimationProgress = new List<float>();
        private List<int> targetMarkerOffsets = new List<int>();

        private float countUp = 0f;
        private float timeElapsed = 0f;
        private int currentBodyIndex = 0;
        private MarkerManager headMarkerManager;

        private void Start()
        {
            LoadBodyConfig();
            CreateHead();
        }

        private void LoadBodyConfig()
        {
            if (jsonConfig == null) return;
            SnakeConfig config = JsonUtility.FromJson<SnakeConfig>(jsonConfig.text);
            bodyDataQueue.AddRange(config.bodyParts);
        }

        private void FixedUpdate()
        {
            timeElapsed += Time.fixedDeltaTime;

            if (bodyDataQueue.Count > currentBodyIndex && countUp >= distanceBetween)
            {
                CreateNewBodyPart();
                countUp = 0f;
            }

            countUp += Time.fixedDeltaTime;
            UpdateCollapseAnimations();
            SnakeMovement();
        }

        private void CreateHead()
        {
            if (snakeBody.Count == 0 && headPrefab != null)
            {
                // Create fake head (invisible)
                GameObject fakeHead = CreateBodyPart(headPrefab, transform.position, transform.rotation, 100, true);
                snakeBody.Add(fakeHead);
                bodyPartMarkerOffsets.Add(0);
                isAnimatingCollapse.Add(false);
                collapseAnimationProgress.Add(0f);
                targetMarkerOffsets.Add(0);

                headMarkerManager = fakeHead.GetComponent<MarkerManager>();
                if (headMarkerManager != null)
                {
                    int estimatedMaxParts = bodyDataQueue.Count + 10;
                    int requiredMarkers = Mathf.CeilToInt(estimatedMaxParts * distanceBetween / (speed * Time.fixedDeltaTime)) + 50;
                    headMarkerManager.SetMaxMarkers(requiredMarkers);
                }

                // Disable renderer on fake head
                var renderer = fakeHead.GetComponent<SpriteRenderer>();
                if (renderer != null) renderer.enabled = false;
            }
        }

        private void CreateNewBodyPart()
        {
            if (currentBodyIndex < bodyDataQueue.Count && headMarkerManager != null)
            {
                SnakeBodyData bodyData = bodyDataQueue[currentBodyIndex];

                // Use real head prefab for first body part, regular body prefab for others
                GameObject prefabToUse = (currentBodyIndex == 0) ? realHeadPrefab : bodyPrefab;

                GameObject newPart = CreateBodyPart(prefabToUse, transform.position, transform.rotation, bodyData.health, false);
                snakeBody.Add(newPart);

                int markerOffset = CalculateMarkerOffset(snakeBody.Count - 1);
                bodyPartMarkerOffsets.Add(markerOffset);
                isAnimatingCollapse.Add(false);
                collapseAnimationProgress.Add(0f);
                targetMarkerOffsets.Add(markerOffset);

                currentBodyIndex++;
            }
        }

        private GameObject CreateBodyPart(GameObject prefab, Vector3 position, Quaternion rotation, int health, bool isHead)
        {
            GameObject part = Instantiate(prefab, position, rotation, transform);

            if (isHead && !part.GetComponent<MarkerManager>())
                part.AddComponent<MarkerManager>();

            if (!part.GetComponent<Rigidbody2D>())
            {
                var rb = part.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0f;
                if (!isHead) rb.isKinematic = true;
            }

            MeteorController meteorController = part.GetComponent<MeteorController>();
            if (meteorController != null)
            {
                meteorController.SetMeteorHealth(health);
                MeteorHealth meteorHealth = meteorController.GetComponent<MeteorHealth>();
                if (meteorHealth != null)
                {
                    meteorHealth.SetSnakeManager(this);
                }
            }

            return part;
        }

        private void UpdateCollapseAnimations()
        {
            for (int i = 0; i < isAnimatingCollapse.Count; i++)
            {
                if (isAnimatingCollapse[i])
                {
                    collapseAnimationProgress[i] += collapseAnimationSpeed * Time.fixedDeltaTime;

                    if (collapseAnimationProgress[i] >= 1f)
                    {
                        collapseAnimationProgress[i] = 1f;
                        bodyPartMarkerOffsets[i] = targetMarkerOffsets[i];
                        isAnimatingCollapse[i] = false;
                    }
                    else
                    {
                        int startOffset = targetMarkerOffsets[i] - CalculateMarkerOffset(1);
                        int endOffset = targetMarkerOffsets[i];
                        float progress = EaseInOutQuad(collapseAnimationProgress[i]);
                        bodyPartMarkerOffsets[i] = Mathf.RoundToInt(Mathf.Lerp(startOffset, endOffset, progress));
                    }
                }
            }
        }

        private float EaseInOutQuad(float t)
        {
            return t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
        }

        private void SnakeMovement()
        {
            if (snakeBody.Count == 0 || headMarkerManager == null) return;

            MoveHead();

            for (int i = 1; i < snakeBody.Count; i++)
            {
                if (i < bodyPartMarkerOffsets.Count)
                {
                    var marker = headMarkerManager.GetMarkerAtOffset(bodyPartMarkerOffsets[i]);
                    if (marker != null)
                    {
                        snakeBody[i].transform.position = marker.Position;
                        snakeBody[i].transform.rotation = marker.Rotation;
                    }
                }
            }
        }

        private void MoveHead()
        {
            if (snakeBody.Count == 0) return;

            float sineValue = Mathf.Sin(timeElapsed * waveFrequency) * waveAmplitude;
            float cosineDerivative = Mathf.Cos(timeElapsed * waveFrequency) * waveFrequency * waveAmplitude;
            float targetAngle = Mathf.Atan2(-speed, cosineDerivative) * Mathf.Rad2Deg;

            var headRb = snakeBody[0].GetComponent<Rigidbody2D>();
            if (headRb != null)
            {
                snakeBody[0].transform.rotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);
                Vector2 direction = new Vector2(cosineDerivative, -speed).normalized;
                headRb.velocity = direction * speed;
            }
        }

        public void RemoveBodyPart(GameObject bodyPart)
        {
            int removedIndex = snakeBody.IndexOf(bodyPart);
            if (removedIndex <= 1) return; // Don't remove fake head (0) or real head (1)

            Debug.Log($"Removing body part at index {removedIndex}, collapse backward: {enableCollapseBackward}");

            if (enableCollapseBackward)
            {
                CollapseBackward(removedIndex);
            }
            else
            {
                SimpleRemoval(removedIndex);
            }
        }

        private void CollapseBackward(int removedIndex)
        {
            Destroy(snakeBody[removedIndex]);
            snakeBody.RemoveAt(removedIndex);
            bodyPartMarkerOffsets.RemoveAt(removedIndex);
            isAnimatingCollapse.RemoveAt(removedIndex);
            collapseAnimationProgress.RemoveAt(removedIndex);
            targetMarkerOffsets.RemoveAt(removedIndex);

            int segmentDistance = CalculateMarkerOffset(1);

            for (int i = 0; i < removedIndex; i++)
            {
                if (i < targetMarkerOffsets.Count)
                {
                    targetMarkerOffsets[i] += segmentDistance;
                    isAnimatingCollapse[i] = true;
                    collapseAnimationProgress[i] = 0f;
                }
            }
        }

        private void SimpleRemoval(int removedIndex)
        {
            Destroy(snakeBody[removedIndex]);
            snakeBody.RemoveAt(removedIndex);
            bodyPartMarkerOffsets.RemoveAt(removedIndex);
            isAnimatingCollapse.RemoveAt(removedIndex);
            collapseAnimationProgress.RemoveAt(removedIndex);
            targetMarkerOffsets.RemoveAt(removedIndex);
        }

        private int CalculateMarkerOffset(int segmentCount)
        {
            return Mathf.RoundToInt(segmentCount * distanceBetween / (speed * Time.fixedDeltaTime));
        }
    }
}