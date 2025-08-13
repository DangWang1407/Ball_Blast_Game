using UnityEngine;
using Game.Controllers;
using Game.Events;

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
            // Apply powwer-up effect
            var weaponController = collision.GetComponent<WeaponController>();

            switch (powerUpData.powerUpType) 
            {
                case PowerUpType.RapidFire:
                    Debug.Log("Applying Rapid Fire Power-Up");
                    weaponController.ApplyRapidFire(powerUpData.duration);
                    break;
                case PowerUpType.DoubleShot:
                    Debug.Log("Applying Double Shot Power-Up");
                    weaponController.ApplyDoubleShot(powerUpData.duration);
                    break;
                case PowerUpType.PowerShot:
                    Debug.Log("Applying Power Shot Power-Up");
                    weaponController.ApplyPowerShot(powerUpData.duration);
                    break;
                case PowerUpType.PierceShot:
                    Debug.Log("Applying Pierce Shot Power-Up");
                    weaponController.ApplyPierceShot(powerUpData.duration);
                    break;
            }
            Destroy(gameObject);
        }
    }
}