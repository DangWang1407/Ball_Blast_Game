using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

namespace Game.Controllers
{
    public class BossAnimation : MonoBehaviour
    {
        private List<int> bodyPartMarkerOffsets = new List<int>();
        private List<bool> isAnimatingCollapse = new List<bool>();
        private List<float> collapseAnimationProgress = new List<float>();
        private List<int> targetMarkerOffsets = new List<int>();
        private List<int> collapseStartOffsets = new List<int>();

        public List<int> BodyPartMarkerOffsets { get => bodyPartMarkerOffsets; set => bodyPartMarkerOffsets = value; }
        public List<bool> IsAnimatingCollapse { get => isAnimatingCollapse; set => isAnimatingCollapse = value; }
        public List<float> CollapseAnimationProgress { get => collapseAnimationProgress; set => collapseAnimationProgress = value; }
        public List<int> TargetMarkerOffsets { get => targetMarkerOffsets; set => targetMarkerOffsets = value; }

        private Boss boss;
        private BossStats bossStats;
        //private BossBuilder bossBuilder;

        [SerializeField]
        private float cascadeDelay = 0.2f;

        public void Initialize(Boss boss)
        {
            this.boss = boss;
            bossStats = GetComponent<BossStats>();
        }

        public void SetAnimation(int bodyPartMarkerOffset, bool isAnimatingCollapse, float collapseAnimationProgress, int targetMarkerOffset)
        {

            BodyPartMarkerOffsets.Add(bodyPartMarkerOffset);
            IsAnimatingCollapse.Add(isAnimatingCollapse);
            CollapseAnimationProgress.Add(collapseAnimationProgress);
            TargetMarkerOffsets.Add(targetMarkerOffset);
            collapseStartOffsets.Add(bodyPartMarkerOffset);
        }

        public void OnFixedUpdate()
        {
            UpdateCollapseAnimations();
        }

        private void UpdateCollapseAnimations()
        {
            for (int i = 0; i < isAnimatingCollapse.Count; i++)
            {
                if (isAnimatingCollapse[i])
                {
                    collapseAnimationProgress[i] += bossStats.CollapseAnimationSpeed * Time.fixedDeltaTime;

                    if (collapseAnimationProgress[i] >= 1f)
                    {
                        collapseAnimationProgress[i] = 1f;
                        bodyPartMarkerOffsets[i] = targetMarkerOffsets[i];
                        isAnimatingCollapse[i] = false;
                    }
                    else
                    {
                        // Use stable start offset captured when animation starts
                        int startOffset = collapseStartOffsets[i];
                        int endOffset = targetMarkerOffsets[i];
                        float progress = EaseInOutQuad(collapseAnimationProgress[i]);
                        bodyPartMarkerOffsets[i] = Mathf.RoundToInt(Mathf.Lerp(startOffset, endOffset, progress));
                    }
                }
            }
        }
        //private Coroutine currentCoroutine;
        public void CollapseBackward(int removedIndex)
        {
            //if(currentCoroutine != null)
            //{
            //    StopCoroutine(currentCoroutine);
            //}
            //Destroy(snakeBody[removedIndex]);
            //snakeBody.RemoveAt(removedIndex);
            bodyPartMarkerOffsets.RemoveAt(removedIndex);
            isAnimatingCollapse.RemoveAt(removedIndex);
            collapseAnimationProgress.RemoveAt(removedIndex);
            targetMarkerOffsets.RemoveAt(removedIndex);
            collapseStartOffsets.RemoveAt(removedIndex);

            // Start collapse in a staggered (one-by-one) fashion for smoother visual effect
            StartCoroutine(CollapseCascade(removedIndex));
        }

        IEnumerator CollapseCascade(int removedIndex)
        {
            int segmentDistance = CalculateMarkerOffset(1);
            for (int i = removedIndex - 1; i >= 0; i--)
            {
                if (i < targetMarkerOffsets.Count)
                {
                    targetMarkerOffsets[i] += segmentDistance;
                    // Capture stable start offset at activation time
                    collapseStartOffsets[i] = bodyPartMarkerOffsets[i];
                    isAnimatingCollapse[i] = true;
                    collapseAnimationProgress[i] = 0f;
                }
                yield return new WaitForSeconds(cascadeDelay);
            }

        }

        public int CalculateMarkerOffset(int segmentCount)
        {
            return Mathf.RoundToInt(segmentCount * bossStats.DistanceBetween / (bossStats.Speed * Time.fixedDeltaTime));
        }

        private float EaseInOutQuad(float t)
        {
            return t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
        }

    }
}
