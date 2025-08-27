using UnityEngine;

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
    }
}