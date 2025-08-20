using TMPro;
using UnityEngine;

namespace Game.Controllers
{
    public class MeteorUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text textHealth;
        private MeteorController meteorController;
        private MeteorHealth meteorHealth;

        public void Initialize(MeteorController meteorController)
        {
            this.meteorController = meteorController;
            if(textHealth == null)
            {
                textHealth = GetComponentInChildren<TMP_Text>();
            }
            meteorHealth = GetComponent<MeteorHealth>();
        }

        public void UpdateHealthDisplay()
        {
            if (textHealth != null)
            {
                
                textHealth.text = meteorHealth.CurrentHealth.ToString();
            }
        }
    }
}