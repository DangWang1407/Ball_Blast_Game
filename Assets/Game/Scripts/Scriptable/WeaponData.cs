namespace Game.Scriptable
{
    using System.Collections.Generic;
    using UnityEngine;
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Game/WeaponData", order = 1)]
    public class WeaponData : ScriptableObject
    {
        [Header("Weapon Stats")]
        public float fireRate = 0.12f;
        public float bulletSpacing = 0.3f;
        public bool pierce = false;

        [Header("Missiles per Shot")]
        public GameObject[] missilePrefabs;

        [Header("Pooling")]
        public string poolNamePrefix = "Missiles";
    }
}