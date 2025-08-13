using UnityEngine;
using Game.Controllers;
using Game.Events;
using Game.Scriptable;

public class  PowerUpController : MonoBehaviour
{
    [SerializeField] private PowerUpData powerUpData;

    private void Start()
    {
        if (powerUpData == null)
        {
            Debug.LogError("PowerUpData is not assigned in PowerUpController.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            EventManager.Trigger(new PowerUpCollectedEvent(powerUpData.powerUpType, powerUpData.duration));
            Destroy(gameObject);
        }
    }
}