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

        private List<float> bodyTimers = new List<float>();
        private List<float> targetBodyTimers = new List<float>();

        public List<GameObject> Body { get => body; set => body = value; }
        public MarkerManager HeadMarkerManager { get => headMarkerManager; private set => headMarkerManager = value; }
        public List<float> BodyTimers { get => bodyTimers; set => bodyTimers = value; }
        public List<float> TargetBodyTimers { get => targetBodyTimers; set => targetBodyTimers = value; }

        public void Initialize(Boss boss)
        {
            this.boss = boss;
            bossStats = GetComponent<BossStats>();
            bossAnimation = GetComponent<BossAnimation>();
        }

        public void OnStart()
        {
            //CreateHead();
        }

        public void OnFixedUpdate()
        {
            if (bossStats.BodyDataQueue.Count > currentBodyIndex && countUp >= bossStats.DistanceBetween)
            {
                CreateBodyPart();
                countUp = 0;
            }
            //for(int i = 0; i < bodyTimers.Count; i++) 
            //{
            //    bodyTimers[i] += Time.fixedDeltaTime;
            //} 
            countUp += Time.fixedDeltaTime;
        }

        private GameObject CreateObject(GameObject prefab, Vector3 position, Quaternion rotation, int health)
        {
            GameObject part = Instantiate(prefab, position, rotation, transform);

            if (!part.GetComponent<Rigidbody2D>())
            {
                var rb = part.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0f;
                rb.isKinematic = true;
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

                GameObject newPart = CreateObject(prefabToUse, transform.position, transform.rotation, bodyData.health);

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


                // need fix
                bodyTimers.Add(0f);
                targetBodyTimers.Add(0f);
            }
        }
    }
}