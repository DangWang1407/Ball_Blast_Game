using UnityEngine;

[CreateAssetMenu(fileName ="PowerUpData", menuName = "Game/PowerUp")]
public class PowerUpData : ScriptableObject
{
    public PowerUpType powerUpType;
    public Sprite icon;
    public float duration;
}