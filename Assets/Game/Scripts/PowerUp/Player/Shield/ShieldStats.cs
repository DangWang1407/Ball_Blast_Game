using UnityEngine;

namespace Game.PowerUp
{
    public class ShieldStats : MonoBehaviour
    {
        //[SerializeField] private GameObject[] shieldPrefabs;
        [SerializeField] private float baseDuration = 20f;
        [SerializeField] private float levelScale = 1.0f;

        //public GameObject GetShieldPrefab(int currentLevel)
        //{
        //    return shieldPrefabs[0];
        //}
        public float GetDuration(int currentLevel)
        {
            return baseDuration + (currentLevel - 1) * levelScale * 5f;
        }
    }
}