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

    public class Boss : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject headPrefab;
        [SerializeField] private GameObject bodyPrefab;

        [Header("Config")]
        [SerializeField] private TextAsset jsonConfig;
        [SerializeField] private float distanceBetween = 0.2f;
        [SerializeField] private float speed = 5f;

        [Header("Sine Wave")]
        [SerializeField] private float waveAmplitude = 2f;
        [SerializeField] private float waveFrequency = 1f;

        private List<GameObject> snakeBody = new List<GameObject>();
        private List<SnakeBodyData> bodyDataQueue = new List<SnakeBodyData>();
        private float countUp = 0f;
        private float timeElapsed = 0f;
        private int currentBodyIndex = 0;

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
            SnakeMovement();
        }

        private void CreateHead()
        {
            if (snakeBody.Count == 0 && headPrefab != null)
            {
                GameObject head = CreateBodyPart(headPrefab, transform.position, transform.rotation, 100); // Head có health mặc định
                snakeBody.Add(head);
            }
        }

        private void CreateNewBodyPart()
        {
            if (currentBodyIndex < bodyDataQueue.Count)
            {
                MarkerManager lastMarker = snakeBody[snakeBody.Count - 1].GetComponent<MarkerManager>();

                if (lastMarker != null && lastMarker.HasMarkers())
                {
                    var marker = lastMarker.GetNextMarker();
                    SnakeBodyData bodyData = bodyDataQueue[currentBodyIndex];

                    GameObject newPart = CreateBodyPart(bodyPrefab, marker.Position, marker.Rotation, bodyData.health);
                    snakeBody.Add(newPart);
                    currentBodyIndex++;
                }
            }
        }

        private GameObject CreateBodyPart(GameObject prefab, Vector3 position, Quaternion rotation, int health)
        {
            GameObject part = Instantiate(prefab, position, rotation, transform);

            // Add components if needed
            if (!part.GetComponent<MarkerManager>())
                part.AddComponent<MarkerManager>();

            if (!part.GetComponent<Rigidbody2D>())
            {
                var rb = part.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0f;
            }

            // Set health cho meteor
            //MeteorController meteorHealth = part.GetComponent<MeteorHealth>();
            //if (meteorHealth != null)
            //{
            //    meteorHealth.SetCustomHealth(health);
            //}
            MeteorController meteorController = part.GetComponent<MeteorController>();
            if (meteorController != null)
            {
                meteorController.SetMeteorHealth(health);
            }

            return part;
        }

        private void SnakeMovement()
        {
            if (snakeBody.Count == 0) return;

            // Head movement với sine wave
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

            // Body parts follow markers
            for (int i = 1; i < snakeBody.Count; i++)
            {
                MarkerManager prevMarker = snakeBody[i - 1].GetComponent<MarkerManager>();
                if (prevMarker != null && prevMarker.HasMarkers())
                {
                    var marker = prevMarker.GetNextMarker();
                    if (marker != null)
                    {
                        snakeBody[i].transform.position = marker.Position;
                        snakeBody[i].transform.rotation = marker.Rotation;
                    }
                }
            }
        }
    }
}