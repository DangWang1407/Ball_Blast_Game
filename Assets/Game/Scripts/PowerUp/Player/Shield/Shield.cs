using Game.Core;
using UnityEngine;

namespace Game.PowerUp
{
    public class Shield : PowerUpEffect
    {
        [SerializeField] private GameObject shieldPrefab;
        private GameObject shield;
        private ShieldStats shieldStats;

        protected override void OnActivate()
        {
            if(gameObject.CompareTag("Player"))
                CreateShield();
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (collision.CompareTag("Player"))
            {
                powerUpType = PowerUpType.Shield;

                if (gameObject.CompareTag("PowerUp"))
                {
                    currentLevel = levelPowerUpManager.GetLevel(PowerUpType.Shield);
                    shieldStats = GetComponent<ShieldStats>();

                    timer = shieldStats.GetDuration(currentLevel);
                }
            }
        }

        protected override void OnDeactivate()
        {
            if (shield != null)
            {
                shield.SetActive(false);
                Destroy(shield);
            }
        }

        private void CreateShield()
        {
            if (shieldPrefab == null)
                shieldPrefab = GameManager.Instance.Data.shieldPrefab;

            if (shieldPrefab != null)
            {
                shield = Instantiate(shieldPrefab, transform);
                shield.transform.localPosition = Vector3.zero;
                shield.tag = "Shield";
                shield.SetActive(true);
            }
        }
    }
}