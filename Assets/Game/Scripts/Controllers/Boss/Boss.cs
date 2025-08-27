using UnityEngine;

namespace Game.Controllers
{
    public class Boss : MonoBehaviour
    {
        private BossStats bossStats;
        private BossBuilder bossBuilder;
        private BossMovement bossMovement;
        private BossAnimation bossAnimation;
        private BossBodyManager bossBodyManager;
        private BossHeath bossHeath;

        private void Awake()
        {
            bossStats = GetComponent<BossStats>();
            bossBuilder = GetComponent<BossBuilder>();
            bossMovement = GetComponent<BossMovement>();
            bossAnimation = GetComponent<BossAnimation>();
            bossBodyManager = GetComponent<BossBodyManager>();
            bossHeath = GetComponent<BossHeath>();

            Initialize();
        }

        private void Initialize()
        {
            bossStats.Initialize(this);
            bossBuilder.Initialize(this);
            bossMovement.Initialize(this);
            bossBodyManager.Initialize(this);
            bossAnimation.Initialize(this);
            bossHeath.Initialize(this);
        }

        private void Start()
        {
            bossBuilder.OnStart();
        }

        private void FixedUpdate()
        {
            bossBuilder.OnFixedUpdate();
            bossAnimation?.OnFixedUpdate();
            bossMovement?.OnFixedUpdate();
            bossHeath?.OnFixedUpdate();
        }

        public void RemoveBodyPart(GameObject bodyPart)
        {
            bossBodyManager?.RemoveBodyPart(bodyPart);
        }
    }
}