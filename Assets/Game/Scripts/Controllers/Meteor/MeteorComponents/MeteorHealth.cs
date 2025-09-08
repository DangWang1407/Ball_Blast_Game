using Game.Events;
using UnityEngine;

namespace Game.Controllers
{
    public class MeteorHealth : MonoBehaviour
    {
        private MeteorController meteorController;
        private MeteorPooling meteorPooling;
        private MeteorUI meteorUI;

        private int currentHealth;
        private int maxHealth;

        private bool useCustomHealth = false;

        public int CurrentHealth { get => currentHealth; set => currentHealth = value; }

        public void Initialize(MeteorController meteorController)
        {
            this.meteorController = meteorController;
            meteorPooling = GetComponent<MeteorPooling>();
            meteorUI = GetComponent<MeteorUI>();
        }

        public void ResetHealth()
        {
            if (!useCustomHealth) 
                currentHealth = maxHealth;
        }

        public void SetHealthBySize(MeteorSize size)
        {
            if (useCustomHealth) return;

            maxHealth = size switch
            {
                MeteorSize.Large => 10,
                MeteorSize.Medium => 5,
                MeteorSize.Small => 2,
                _ => 10
            };
            currentHealth = maxHealth;
        }

        public void SetCustomHealth(int health)
        {
            useCustomHealth = true;
            maxHealth = health;
            currentHealth = health;
        }

        // need fix 
        private Boss boss;
        public void SetBoss(Boss boss)
        {
            this.boss = boss;
        }

        private BodyData bodyData;
        public void SetBodyData(BodyData bodyData)
        {
            this.bodyData = bodyData;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                int scoreValue = meteorController.MeteorSize switch
                {
                    MeteorSize.Large => 100,
                    MeteorSize.Medium => 50,
                    MeteorSize.Small => 25,
                    _ => 0
                };

                EventManager.Trigger(new ScoreChangeEvent(ScoreReason.MeteorDestroyed, scoreValue));

                //need fix
                //var snakeManager = GetComponent<SnakeManager>();
                if (boss != null)
                {
                    if (bodyData != null && bodyData.powerUpType != PowerUp.PowerUpType.None)
                    {
                        Debug.Log("Trigger new power up event: " + bodyData.powerUpType);
                        EventManager.Trigger(new SpecificPowerUpSpawnEvent(transform.position, bodyData.powerUpType));
                    }

                    boss.RemoveBodyPart(gameObject);
                    return;
                }

                if (meteorController.MeteorSize != MeteorSize.Small)
                {
                    int childHealth = Mathf.Max(1, maxHealth / 2);
                    EventManager.Trigger(new SplitMeteorEvent(transform.position, meteorController.MeteorSize, childHealth));                   
                }
                // Notify systems that a meteor was destroyed
                EventManager.Trigger(new MeteorDestroyedEvent(meteorController.MeteorSize, transform.position));
                EventManager.Trigger(new PowerUpSpawnEvent(transform.position, meteorController.MeteorSize));
                meteorPooling.DestroyMeteor();
            }
            meteorUI.UpdateHealthDisplay();          
        }
    }
}
