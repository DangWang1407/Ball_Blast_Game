using UnityEngine;
using Game.Events;
using Game.PowerUp;
using UnityEngine.UI;
namespace Game.Scriptable
{
    [CreateAssetMenu(fileName = "PowerUpData", menuName = "Game/PowerUp")]
    public class PowerUpData : ScriptableObject
    {
        public PowerUpType powerUpType;
        public string powerUpName;
        public string powerUpDescription;
        public Image powerUpImage;
    }
}
