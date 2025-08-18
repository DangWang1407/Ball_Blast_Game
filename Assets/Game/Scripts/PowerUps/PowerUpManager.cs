using Game.Controllers;
using Game.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PowerUps
{
    public class PowerUpManager : MonoBehaviour
    {
        [SerializeField] private WeaponController weaponController;
        [SerializeField] private PlayerController playerController;

        private Dictionary<PowerUpType, IPowerUpWeapon> weaponPowerUps = new Dictionary<PowerUpType, IPowerUpWeapon>();
        private Dictionary<PowerUpType, IPowerUpDefend> defendPowerUps = new Dictionary<PowerUpType, IPowerUpDefend>();

        private void Start()
        {
            InitializePowerUps();
            EventManager.Subscribe<PowerUpCollectedEvent>(OnPowerUpCollected);
        }

        private void InitializePowerUps()
        {
            weaponPowerUps[PowerUpType.Homing] = new HomingPowerUp();
            weaponPowerUps[PowerUpType.RapidFire] = new RapidFirePowerUp();
            weaponPowerUps[PowerUpType.DiagonalFire] = new DiagonalFirePowerUp();
            weaponPowerUps[PowerUpType.PierceShot] = new PierceShotPowerUp();
            weaponPowerUps[PowerUpType.BounceShot] = new BounceShotPowerUp();
            weaponPowerUps[PowerUpType.BurstShot] = new BurstShotPowerUp();
            weaponPowerUps[PowerUpType.DamageBoost] = new DamageBoostPowerUp();
            weaponPowerUps[PowerUpType.DoubleShot] = new DoubleShotPowerUp();
            weaponPowerUps[PowerUpType.PowerShot] = new PowerShotPowerUp();

            defendPowerUps[PowerUpType.Invisible] = new InvisiblePowerUp();
            defendPowerUps[PowerUpType.Shield] = new ShieldPowerUp();
        }

        private void OnPowerUpCollected(PowerUpCollectedEvent powerUpEvent)
        {
            if (weaponPowerUps.TryGetValue(powerUpEvent.PowerUpType, out var weaponPowerup))
            {
                weaponPowerup.Apply(weaponController, powerUpEvent.Duration);
            }
            if(defendPowerUps.TryGetValue(powerUpEvent.PowerUpType, out var defendPowerup))
            {
                defendPowerup.Apply(playerController, powerUpEvent.Duration);
            }
        }

        private void OnDestroy()
        {
            EventManager.Unsubscribe<PowerUpCollectedEvent>(OnPowerUpCollected);
        }
    }
}