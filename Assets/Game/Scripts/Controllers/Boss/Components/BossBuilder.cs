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
            if (bossStats.BodyDataQueue.Count > currentBodyIndex && countUp >= bossStats.DistanceBetween)
            {
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

                bossAnimation.SetAnimation(0, false, 0f, 0);

                headMarkerManager = fakeHead.GetComponent<MarkerManager>();
                if (headMarkerManager != null)
                {
                    int estimatedMaxParts = bossStats.BodyDataQueue.Count + 10;
                    int requiredMarkers = Mathf.CeilToInt(estimatedMaxParts * bossStats.DistanceBetween / (bossStats.Speed * Time.fixedDeltaTime)) + 50;

                    Debug.Log("Required markers: " + requiredMarkers);
                    headMarkerManager.SetMaxMarkers(requiredMarkers);
                }
            }
        }

        private void CreateBodyPart()
        {
            if (currentBodyIndex < bossStats.BodyDataQueue.Count)
            {
                BodyData bodyData = bossStats.BodyDataQueue[currentBodyIndex];

                //GameObject prefabToUse = (currentBodyIndex == 0) ? bossStats.RealHeadPrefab : bossStats.BodyPrefab;

                GameObject prefabToUse;
                if (currentBodyIndex == 0)
                {
                    prefabToUse = bossStats.RealHeadPrefab;
                }
                else
                {
                    prefabToUse = bossStats.GetBodyPrefab(bodyData.prefabIndex);
                }

                //Debug.Log(bodyData.health);

                GameObject newPart = CreateObject(prefabToUse, transform.position, transform.rotation, bodyData.health, false);

                MeteorHealth meteorHealth = newPart.GetComponent<MeteorHealth>();
                if (meteorHealth != null)
                {
                    //Debug.Log("Meteor health exists");
                    meteorHealth.SetBodyData(bodyData);
                }

                body.Add(newPart);

                int markerOffset = bossAnimation.CalculateMarkerOffset(body.Count - 1);

                bossAnimation.SetAnimation(markerOffset, false, 0f, markerOffset);

                currentBodyIndex++;
            }
        }
    }
}