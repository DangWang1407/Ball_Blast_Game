using UnityEngine;

namespace Game.Controllers
{
    public class BossBodyManager : MonoBehaviour
    {
        private Boss boss;
        private BossStats bossStats;
        private BossBuilder bossBuilder;
        private BossAnimation bossAnimation;

        public void Initialize(Boss boss)
        {
            this.boss = boss;
            bossStats = GetComponent<BossStats>();
            bossBuilder = GetComponent<BossBuilder>();
            bossAnimation = GetComponent<BossAnimation>();
        }

        public void RemoveBodyPart(GameObject bodyPart)
        {
            int removedIndex = bossBuilder.Body.IndexOf(bodyPart);

            Debug.Log($"Removing body part at index {removedIndex}, collapse backward: {bossStats.EnableCollapseBackward}");

            if (removedIndex < 0)
            {
                Debug.LogWarning("[BossBodyManager] Tried to remove a body part that is not in the list.");
                return;
            }

            if (bossStats.EnableCollapseBackward)
            {
                Destroy(bossBuilder.Body[removedIndex]);
                bossBuilder.Body.RemoveAt(removedIndex);
                bossAnimation.CollapseBackward(removedIndex);
                Debug.Log("Remain body number " + bossBuilder.Body.Count);
            }
            else
            {
                Destroy(bossBuilder.Body[removedIndex]);
                bossBuilder.Body.RemoveAt(removedIndex);
            }
        }


    }
}
