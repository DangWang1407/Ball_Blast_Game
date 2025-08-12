using UnityEngine;
using Game.Controllers;

public class  PowerUpController : MonoBehaviour
{
    [SerializeField] private PowerUpData powerUpData;

    private void Start()
    {
        if (powerUpData == null)
        {
            Debug.LogError("PowerUpData is not assigned in PowerUpController.");
        }

        this.gameObject.GetComponent<SpriteRenderer>().sprite = powerUpData.icon;
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
                    weaponController.ApplyDoubleShot(powerUpData.duration);
                    break;
            }
            Destroy(gameObject);
        }
    }
}