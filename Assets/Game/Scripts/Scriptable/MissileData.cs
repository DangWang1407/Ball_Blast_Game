using UnityEngine;
using Game.Events;

namespace Game.Scriptable
{
    [CreateAssetMenu(fileName = "MissileData", menuName = "Game/Missile")]
    public class MissileData : ScriptableObject
    {
        [Header("Missile Settings")]
        public float missileSpeed = 6f;
        public float bulletScale = 0.5f;
        public int damage = 1;
        public bool canHoming = false;
        public bool canBounce = false;
        public bool canPierce = false;
    }
}