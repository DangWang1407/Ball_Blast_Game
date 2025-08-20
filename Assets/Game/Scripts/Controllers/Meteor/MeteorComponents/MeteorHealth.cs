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

        public int CurrentHealth { get => currentHealth; set => currentHealth = value; }

        public void Initialize(MeteorController meteorController)
        {
            this.meteorController = meteorController;
            meteorPooling = GetComponent<MeteorPooling>();
            meteorUI = GetComponent<MeteorUI>();
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
        }

        public void SetHealthBySize(MeteorSize size)
        {
            maxHealth = size switch
            {
                MeteorSize.Large => 10,
                MeteorSize.Medium => 5,
                MeteorSize.Small => 2,
                _ => 10
            };
            currentHealth = maxHealth;
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

                if(meteorController.MeteorSize != MeteorSize.Small)
                {
                    //MeteorSpawnerController.Instance?.SpawnSplitMeteors(transform.position, meteorController.MeteorSize);
                    EventManager.Trigger(new SplitMeteorEvent(transform.position, meteorController.MeteorSize));
                    
                }
                EventManager.Trigger(new PowerUpSpawnEvent(transform.position, meteorController.MeteorSize));
                meteorPooling.DestroyMeteor();
            }
            meteorUI.UpdateHealthDisplay();          
        }
    }
}