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

        private void Awake()
        {
            bossStats = GetComponent<BossStats>();
            bossBuilder = GetComponent<BossBuilder>();
            bossMovement = GetComponent<BossMovement>();
            bossAnimation = GetComponent<BossAnimation>();
            bossBodyManager = GetComponent<BossBodyManager>();

            Initialize();
        }

        private void Initialize()
        {
            bossStats.Initialize(this);
            bossBuilder.Initialize(this);
            bossMovement.Initialize(this);
            bossBodyManager.Initialize(this);
            bossAnimation.Initialize(this);
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
        }

        public void RemoveBodyPart(GameObject bodyPart)
        {
            bossBodyManager?.RemoveBodyPart(bodyPart);
        }
    }
}