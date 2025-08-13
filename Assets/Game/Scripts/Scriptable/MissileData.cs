using UnityEngine;
using Game.Events;

namespace Game.Scriptable
{
    [CreateAssetMenu(fileName = "MissileData", menuName = "Game/Missile")]
    public class MissileData : ScriptableObject
    {
        [Header("Missile Settings")]
        public GameObject missilePrefab;
        public MissileType missileType = MissileType.Standard;
        public float speed = 8f;
        public float scale = 0.5f;
    }
}