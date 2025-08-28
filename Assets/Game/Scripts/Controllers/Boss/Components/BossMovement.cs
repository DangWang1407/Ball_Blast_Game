using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Controllers
{
    public class BossMovement : MonoBehaviour
    {
        private Boss boss;
        private BossBuilder bossBuilder;
        private BossStats bossStats;
        private BossAnimation BossAnimation;

        private float timeElapsed = 0;

        public void Initialize(Boss boss)
        {
            this.boss = boss;
            bossBuilder = GetComponent<BossBuilder>();
            bossStats = GetComponent<BossStats>();
            BossAnimation = GetComponent<BossAnimation>();
        }

        public void OnFixedUpdate()
        {
            timeElapsed += Time.fixedDeltaTime;
            Movement();
        }

        private void Movement()
        {
            if (bossBuilder.Body.Count == 0 || bossBuilder.HeadMarkerManager == null) return;
            MoveHead();

            for (int i = 1; i < bossBuilder.Body.Count; i++)
            {
                if (i < BossAnimation.BodyPartMarkerOffsets.Count)
                {
                    var marker = bossBuilder.HeadMarkerManager.GetMarkerAtOffset(BossAnimation.BodyPartMarkerOffsets[i]);
                    if (marker != null)
                    {
                        bossBuilder.Body[i].transform.position = marker.Position;
                        if(i == 0)
                            bossBuilder.Body[i].transform.rotation = marker.Rotation;
                    }
                }
            }
        }

        private void MoveHead()
        {
            if (bossBuilder.Body.Count == 0) return;

            float sineValue = Mathf.Sin(timeElapsed * bossStats.WaveFrequency) * bossStats.WaveAmplitude;
            float cosineDerivative = Mathf.Cos(timeElapsed * bossStats.WaveFrequency) * bossStats.WaveFrequency * bossStats.WaveAmplitude;
            float targetAngle = Mathf.Atan2(-bossStats.Speed, cosineDerivative) * Mathf.Rad2Deg;

            var headRb = bossBuilder.Body[0].GetComponent<Rigidbody2D>();
            if (headRb != null)
            {
                bossBuilder.Body[0].transform.rotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);
                Vector2 direction = new Vector2(cosineDerivative, -bossStats.Speed).normalized;
                headRb.velocity = direction * bossStats.Speed;
            }
        }

        #if UNITY_EDITOR

        [Header("Zigzag Path")]
        public Vector2 spawnPoint = new Vector2(0, 5);
        public float width = 3.5f;
        public float height = 1.5f;
        public float rangeTime = 10f;
        public int numCycles = 5;

        private Dictionary<int, (Vector3, Vector3, Vector3, Vector3)> controlPoints = new();

        private void OnDrawGizmosSelected()
        {
            controlPoints.Clear();
            for (int n = 0; n < numCycles; n++)
            {
                controlPoints[n] = CalculateControlPoints(n);
            }

            Handles.color = Color.red;
            Vector3 lastPoint = GetBezierPoint(0f);

            float totalTime = numCycles * rangeTime;
            int segments = numCycles * 50;

            for (int i = 0; i < segments; i++)
            {
                float t = (i / (float)segments) * totalTime;
                Vector3 currentPoint = GetBezierPoint(t);
                Handles.DrawLine(lastPoint, currentPoint);
                lastPoint = currentPoint;
            }

            Handles.color = Color.yellow;
            foreach (var kvp in controlPoints)
            {
                var (An, Bn, Cn, Dn) = kvp.Value;
                Handles.DrawWireCube(An, Vector3.one * 0.2f);
                Handles.DrawWireCube(Bn, Vector3.one * 0.2f);
                Handles.DrawWireCube(Cn, Vector3.one * 0.2f);
                Handles.DrawWireCube(Dn, Vector3.one * 0.2f);
            }
        }

        private Vector3 GetBezierPoint(float t)
        {
            int n = Mathf.FloorToInt(t / rangeTime);
            float normalizedTime = (t - n * rangeTime) / rangeTime;

            var (An, Bn, Cn, Dn) = controlPoints[n];
            return GetBezierPoint1(normalizedTime, An, Bn, Cn, Dn);
        }

        private (Vector3, Vector3, Vector3, Vector3) CalculateControlPoints(int n)
        {
            float y1 = spawnPoint.y - n * height;
            float y2 = spawnPoint.y - (n + 1) * height;
            float xOffset = Mathf.Pow(-1, n) * width;

            return (
                new Vector3(spawnPoint.x, y1, 0),     // An
                new Vector3(xOffset, y1, 0),         // Bn  
                new Vector3(xOffset, y2, 0),         // Cn
                new Vector3(spawnPoint.x, y2, 0)     // Dn
            );
        }

        private Vector3 GetBezierPoint1(float t, Vector3 An, Vector3 Bn, Vector3 Cn, Vector3 Dn)
        {
            float u = 1 - t;
            return u * u * u * An + 3 * u * u * t * Bn + 3 * u * t * t * Cn + t * t * t * Dn;
        }
        #endif
    }
}