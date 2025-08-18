using UnityEngine;

namespace Game.Controllers
{
    public static class WeaponStats
    {
        public static float missileSpeed = 6f;
        public static float fireRate = 0.2f;
        public static int bulletCount = 1;
        public static float bulletScale = 0.5f;
        public static bool pierce = false;
        public static bool burst = false;
        public static int damage = 1;
        public static bool homing = false;
        public static bool diagonalFire = false;
        public static bool bounceShot = false;
    }
}