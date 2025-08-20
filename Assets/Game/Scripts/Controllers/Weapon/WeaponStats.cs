using UnityEngine;

namespace Game.Controllers
{
    public static class WeaponStats
    {
        //for missile:
        public static float missileSpeed = 6f;
        public static bool pierce = false;
        public static float bulletScale = 0.5f;
        public static int damage = 1;
        public static bool homing = false;
        public static bool bounceShot = false;

        //for weapon
        public static float fireRate = 0.2f;
        public static int bulletCount = 1;
        public static bool burst = false;
        public static bool diagonalFire = false;
        public static int burstCount = 1;
        public static float burstDelay = 0.05f;
    }
}