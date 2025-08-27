using System.Collections.Generic;
using UnityEngine;

namespace Game.Controllers
{
    public class BossBuilder : MonoBehaviour
    {
        Boss boss;
        BossStats bossStats;
        BossAnimation bossAnimation;

        private List<GameObject> body = new List<GameObject>();
        private MarkerManager headMarkerManager;
        private int currentBodyIndex = 0;
        private float countUp = 0;

        public List<GameObject> Body { get => body; set => body = value; }
        public MarkerManager HeadMarkerManager { get => headMarkerManager; private set => headMarkerManager = value; }

        public void Initialize(Boss boss)
        {
            this.boss = boss;
            bossStats = GetComponent<BossStats>();
            bossAnimation = GetComponent<BossAnimation>();
        }

        public void OnStart()
        {
            CreateHead();
        }

        public void OnFixedUpdate()
        {
            if (bossStats.BodyDataQueue.Count > currentBodyIndex && countUp >= bossStats.DistanceBetween) { 
                CreateBodyPart();
                countUp = 0;
            }
            countUp += Time.fixedDeltaTime;
        }

        private GameObject CreateObject(GameObject prefab, Vector3 position, Quaternion rotation, int health, bool isHead)
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
                    meteorHealth.SetBoss(boss);
                }
            }
            return part;
        }

        private void CreateHead()
        {
            if (body.Count == 0 && bossStats.HeadPrefab != null)
            {
                GameObject fakeHead = CreateObject(bossStats.HeadPrefab, transform.position, transform.rotation, 100, true);

                body.Add(fakeHead);

                bossAnimation.BodyPartMarkerOffsets.Add(0);
                bossAnimation.IsAnimatingCollapse.Add(false);
                bossAnimation.CollapseAnimationProgress.Add(0f);
                bossAnimation.TargetMarkerOffsets.Add(0);

                headMarkerManager = fakeHead.GetComponent<MarkerManager>();
                if (headMarkerManager != null)
                {
                    int estimatedMaxParts = bossStats.BodyDataQueue.Count + 10;
                    int requiredMarkers = Mathf.CeilToInt(estimatedMaxParts * bossStats.DistanceBetween / (bossStats.Speed * Time.fixedDeltaTime)) + 50;

                    Debug.Log("Required markers: ");
                    headMarkerManager.SetMaxMarkers(requiredMarkers);
                }
            }
        }

        private void CreateBodyPart()
        {
            if (currentBodyIndex < bossStats.BodyDataQueue.Count) 
            {
                SnakeBodyData bodyData = bossStats.BodyDataQueue[currentBodyIndex];

                GameObject prefabToUse = (currentBodyIndex == 0) ? bossStats.RealHeadPrefab : bossStats.BodyPrefab;

                GameObject newPart = CreateObject(prefabToUse, transform.position, transform.rotation, bodyData.health, false);
                body.Add(newPart);

                int markerOffset = bossAnimation.CalculateMarkerOffset(body.Count - 1);
                bossAnimation.BodyPartMarkerOffsets.Add(markerOffset);
                bossAnimation.IsAnimatingCollapse.Add(false);
                bossAnimation.CollapseAnimationProgress.Add(0f);
                bossAnimation.TargetMarkerOffsets.Add(markerOffset);

                currentBodyIndex++;
            }
        }
    }
}