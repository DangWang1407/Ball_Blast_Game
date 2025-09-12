using TMPro;
using UnityEngine;
using Game.UI;

namespace Game.Controllers
{
    public class MeteorUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text textHealth;
        private MeteorController meteorController;
        private MeteorHealth meteorHealth;
        
        private MeteorHealthUIBinder meteorHealthUIBinder;

        public void Initialize(MeteorController meteorController)
        {
            this.meteorController = meteorController;

            if (textHealth == null)
            {
                textHealth = GetComponentInChildren<TMP_Text>();
            }
            meteorHealth = GetComponent<MeteorHealth>();


            meteorHealthUIBinder = GetComponent<MeteorHealthUIBinder>();
            if(meteorHealthUIBinder != null) textHealth = null;
        }

        public void UpdateHealthDisplay()
        {
            if(meteorHealthUIBinder != null)
            {
                meteorHealthUIBinder.SetHealth(meteorHealth.CurrentHealth);
                return;
            }


            if (textHealth != null)
            {

                textHealth.text = meteorHealth.CurrentHealth.ToString();
            }
        }
    }
}
