using UnityEngine;

namespace Game.Controllers
{
    public class BossHeath : MonoBehaviour
    {
        private Boss boss;
        private BossBuilder bossBuilder;

        public void Initialize(Boss boss)
        {
            this.boss = boss;
            bossBuilder = GetComponent<BossBuilder>();
        }

        public void OnFixedUpdate()
        {
            if(bossBuilder.Body.Count <= 2)
            {
                Debug.Log("Boss died");
            }
        }
    }
}