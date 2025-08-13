using UnityEngine;
using Game.Events;
namespace Game.Scriptable
{
    [CreateAssetMenu(fileName = "PowerUpData", menuName = "Game/PowerUp")]
    public class PowerUpData : ScriptableObject
    {
        public PowerUpType powerUpType;
        public float duration;
    }
}
