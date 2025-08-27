using System.Collections.Generic;
using UnityEngine;
using Game.PowerUp;

namespace Game.Controllers
{
    [System.Serializable]
    public class BodyData
    {
        public int health;
        public PowerUpType powerUpType = PowerUpType.None;
        public int prefabIndex;                                                           
    }

    [System.Serializable]
    public class BossConfig
    {
        public BodyData[] bodyParts;
    }
    public class BossStats : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject headPrefab; // Fake head (invisible)
        [SerializeField] private GameObject realHeadPrefab; // Real head prefab
        [SerializeField] private GameObject[] bodyPrefabs;

        [Header("Config")]
        [SerializeField] private TextAsset jsonConfig;
        [SerializeField] private float distanceBetween = 0.8f;
        [SerializeField] private float speed = 0.9f;

        [Header("Sine Wave")]
        [SerializeField] private float waveAmplitude = 15f;
        [SerializeField] private float waveFrequency = 0.5f;

        [Header("Collapse Settings")]
        [SerializeField] private bool enableCollapseBackward = true;
        [SerializeField] private float collapseAnimationSpeed = 5f;

        private List<BodyData> bodyDataQueue = new List<BodyData>();

        public GameObject HeadPrefab { get => headPrefab; private set => headPrefab = value; }
        public GameObject RealHeadPrefab { get => realHeadPrefab; private set => realHeadPrefab = value; }
        public GameObject[] BodyPrefabs { get => bodyPrefabs; private set => bodyPrefabs = value; }
        //public TextAsset JsonConfig { get => jsonConfig; private set => jsonConfig = value; }
        public float DistanceBetween { get => distanceBetween; private set => distanceBetween = value; }
        public float Speed { get => speed; private set => speed = value; }
        public float WaveAmplitude { get => waveAmplitude; private set => waveAmplitude = value; }
        public float WaveFrequency { get => waveFrequency; private set => waveFrequency = value; }
        public bool EnableCollapseBackward { get => enableCollapseBackward; private set => enableCollapseBackward = value; }
        public float CollapseAnimationSpeed { get => collapseAnimationSpeed; private set => collapseAnimationSpeed = value; }
        public List<BodyData> BodyDataQueue { get => bodyDataQueue; set => bodyDataQueue = value; }

        private Boss boss;
        public void Initialize(Boss boss)
        {
            this.boss = boss; 
            LoadBodyConfig();
        }

        private void LoadBodyConfig()
        {
            if (jsonConfig == null) return;
            BossConfig config = JsonUtility.FromJson<BossConfig>(jsonConfig.text);
            bodyDataQueue.AddRange(config.bodyParts);
        }

        public GameObject GetBodyPrefab(int prefabIndex)
        {
            if(prefabIndex < 0 || prefabIndex >= bodyPrefabs.Length)
            {
                Debug.Log("Index is invalid");
                return null;
            }
            return bodyPrefabs[prefabIndex];
        }
    }
}