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

        private void Awake()
        {
            controlPoints.Clear();
            for (int n = 0; n < numCycles; n++)
            {
                controlPoints[n] = CalculateControlPoints(n);
            }
        }

        public void Initialize(Boss boss)
        {
            this.boss = boss;
            bossBuilder = GetComponent<BossBuilder>();
            bossStats = GetComponent<BossStats>();
            BossAnimation = GetComponent<BossAnimation>();
        }

        public void OnFixedUpdate()
        {
            Movement();
        }

        private void Movement()
        {
            if (bossBuilder.Body.Count == 0) return;

            for (int i = 0; i < bossBuilder.Body.Count; i++)
            {
                if (i < BossAnimation.BodyPartMarkerOffsets.Count && !BossAnimation.IsAnimatingCollapse[i])
                {
                    Vector3 currentPos = GetBezierPoint(bossBuilder.BodyTimers[i]); // 2.7 là distanceBetween
                    Vector3 nextPos = GetBezierPoint(bossBuilder.BodyTimers[i] + Time.fixedDeltaTime);

                    var RB = bossBuilder.Body[i].GetComponent<Rigidbody2D>();
                    if (RB != null)
                    {
                        Vector2 direction = (nextPos - currentPos).normalized;
                        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                        //bossBuilder.Body[i].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                        RB.velocity = direction * bossStats.Speed;
                    }
                    bossBuilder.BodyTimers[i] += Time.fixedDeltaTime;
                }

                if (i < BossAnimation.BodyPartMarkerOffsets.Count && BossAnimation.IsAnimatingCollapse[i])
                {
                    Vector3 currentPos = GetBezierPoint(bossBuilder.BodyTimers[i]); // 2.7 là distanceBetween
                    Vector3 nextPos = GetBezierPoint(bossBuilder.BodyTimers[i] - Time.fixedDeltaTime);

                    var RB = bossBuilder.Body[i].GetComponent<Rigidbody2D>();
                    if (RB != null)
                    {
                        Vector2 direction = (nextPos - currentPos).normalized;
                        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                        //bossBuilder.Body[i].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                        RB.velocity = direction * bossStats.Speed;
                    }
                    bossBuilder.BodyTimers[i] -= Time.fixedDeltaTime;
                    if (bossBuilder.BodyTimers[i] <= bossBuilder.TargetBodyTimers[i] && BossAnimation.IsAnimatingCollapse[i])
                    {
                        BossAnimation.IsAnimatingCollapse[i] = false;
                        bossBuilder.BodyTimers[i] = bossBuilder.TargetBodyTimers[i];
                    }
                }
            }
        }

        [Header("Zigzag Path")]
        public Vector2 spawnPoint = new Vector2(0, 5);
        public float width = 3.5f;
        public float height = 1.5f;
        public float rangeTime = 10f;
        public int numCycles = 5;
        public float Offset = 0.2f;

        private Dictionary<int, (Vector3, Vector3, Vector3, Vector3)> controlPoints = new();

        private void OnDrawGizmosSelected()
        {
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

            Handles.color = Color.cyan;
            Vector3 lastRealPoint = SimulateRealMovement(0f);

            float currentTime = rangeTime;
            int realSegments = Mathf.FloorToInt(currentTime / Time.fixedDeltaTime);

            for (int i = 1; i <= realSegments; i++)
            {
                float t = i * Time.fixedDeltaTime;
                Vector3 currentRealPoint = SimulateRealMovement(t);
                Handles.DrawLine(lastRealPoint, currentRealPoint);
                lastRealPoint = currentRealPoint;
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

        private Vector3 SimulateRealMovement(float t)
        {
            Vector3 position = spawnPoint;
            float deltaTime = Time.fixedDeltaTime;

            for (float time = 0; time < t; time += deltaTime)
            {
                Vector3 currentTarget = GetBezierPoint(time);
                Vector3 nextTarget = GetBezierPoint(time + deltaTime);

                Vector2 direction = (nextTarget - currentTarget).normalized;
                Vector2 velocity = direction * 0.3f;

                position += (Vector3)(velocity * deltaTime);
            }

            return position;
        }

        private Vector3 GetBezierPoint(float t)
        {
            /////
            if (t < 0) t = 0;
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
                new Vector3(xOffset, y1 + Offset, 0),         // Bn  
                new Vector3(xOffset, y2 - Offset, 0),         // Cn
                new Vector3(spawnPoint.x, y2, 0)     // Dn
            );
        }

        private Vector3 GetBezierPoint1(float t, Vector3 An, Vector3 Bn, Vector3 Cn, Vector3 Dn)
        {
            float u = 1 - t;
            return u * u * u * An + 3 * u * u * t * Bn + 3 * u * t * t * Cn + t * t * t * Dn;
        }
    }
}