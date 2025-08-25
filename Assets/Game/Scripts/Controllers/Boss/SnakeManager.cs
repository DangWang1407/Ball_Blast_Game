//SnakeManager.cs - Optimized version
using System.Collections.Generic;
using UnityEngine;

namespace Game.Controllers
{
    public class SnakeManager : MonoBehaviour
    {
        [SerializeField] private float distanceBetween = 0.2f;
        [SerializeField] private float speed = 5f;
        [SerializeField] private List<GameObject> bodyParts = new List<GameObject>();

        // Sine wave parameters
        [SerializeField] private float waveAmplitude = 2f;
        [SerializeField] private float waveFrequency = 1f;

        private List<GameObject> snakeBody = new List<GameObject>();
        private float countUp = 0f;
        private float timeElapsed = 0f;

        private void Start()
        {
            CreateFirstBodyPart();
        }

        private void FixedUpdate()
        {
            timeElapsed += Time.fixedDeltaTime;

            if (bodyParts.Count > 0 && countUp >= distanceBetween)
            {
                CreateNewBodyPart();
                countUp = 0f;
            }

            countUp += Time.fixedDeltaTime;
            SnakeMovement();
        }

        private void CreateFirstBodyPart()
        {
            if (snakeBody.Count == 0 && bodyParts.Count > 0)
            {
                GameObject head = CreateBodyPart(bodyParts[0], transform.position, transform.rotation);
                snakeBody.Add(head);
                bodyParts.RemoveAt(0);
            }
        }

        private void CreateNewBodyPart()
        {
            if (bodyParts.Count > 0)
            {
                MarkerManager lastMarker = snakeBody[snakeBody.Count - 1].GetComponent<MarkerManager>();

                if (lastMarker != null && lastMarker.HasMarkers())
                {
                    var marker = lastMarker.GetNextMarker();
                    GameObject newPart = CreateBodyPart(bodyParts[0], marker.Position, marker.Rotation);
                    snakeBody.Add(newPart);
                    bodyParts.RemoveAt(0);
                }
            }
        }

        private GameObject CreateBodyPart(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            GameObject part = Instantiate(prefab, position, rotation, transform);

            if (!part.GetComponent<MarkerManager>())
                part.AddComponent<MarkerManager>();

            if (!part.GetComponent<Rigidbody2D>())
            {
                var rb = part.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0f;
            }

            return part;
        }

        private void SnakeMovement()
        {
            if (snakeBody.Count == 0) return;

            float sineValue = Mathf.Sin(timeElapsed * waveFrequency) * waveAmplitude;
            float cosineDerivative = Mathf.Cos(timeElapsed * waveFrequency) * waveFrequency * waveAmplitude;

            float targetAngle = Mathf.Atan2(-speed, cosineDerivative) * Mathf.Rad2Deg;

            var headRb = snakeBody[0].GetComponent<Rigidbody2D>();
            if (headRb != null)
            {
                snakeBody[0].transform.rotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);

                // Move downward with sine wave
                Vector2 direction = new Vector2(cosineDerivative, -speed).normalized;
                headRb.velocity = direction * speed;
            }


            // Move body parts following markers
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
