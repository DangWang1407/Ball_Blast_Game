using UnityEngine;
using Game.Events;

[CreateAssetMenu(fileName ="PowerUpData", menuName = "Game/PowerUp")]
public class PowerUpData : ScriptableObject
{
    public PowerUpType powerUpType;
    public float duration;
}