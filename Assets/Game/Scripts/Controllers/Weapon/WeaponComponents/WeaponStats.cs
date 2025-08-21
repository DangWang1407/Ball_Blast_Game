using UnityEngine;

namespace Game.Controllers
{
    public class WeaponStats : MonoBehaviour
    {
        private WeaponController weaponController;

        private float fireRate = 0.2f;
        private int bulletCount = 1;
        private bool burst = false;
        private int burstCount = 1;
        private float burstDelay = 0.05f;

        public float FireRate { get => fireRate; set => fireRate = value; }
        public int BulletCount { get => bulletCount; set => bulletCount = value; }
        public bool Burst { get => burst; set => burst = value; }
        public int BurstCount { get => burstCount; set => burstCount = value; }
        public float BurstDelay { get => burstDelay; set => burstDelay = value; }

        public void Initialize(WeaponController weaponController)
        {
            this.weaponController = weaponController;
        }
    }
}