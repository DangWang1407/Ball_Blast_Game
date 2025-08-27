using System.Collections.Generic;
using UnityEngine;

namespace Game.Controllers
{
    public class BossStats : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject headPrefab; // Fake head (invisible)
        [SerializeField] private GameObject realHeadPrefab; // Real head prefab
        [SerializeField] private GameObject bodyPrefab;

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

        private List<SnakeBodyData> bodyDataQueue = new List<SnakeBodyData>();

        public GameObject HeadPrefab { get => headPrefab; private set => headPrefab = value; }
        public GameObject RealHeadPrefab { get => realHeadPrefab; private set => realHeadPrefab = value; }
        public GameObject BodyPrefab { get => bodyPrefab; private set => bodyPrefab = value; }
        //public TextAsset JsonConfig { get => jsonConfig; private set => jsonConfig = value; }
        public float DistanceBetween { get => distanceBetween; private set => distanceBetween = value; }
        public float Speed { get => speed; private set => speed = value; }
        public float WaveAmplitude { get => waveAmplitude; private set => waveAmplitude = value; }
        public float WaveFrequency { get => waveFrequency; private set => waveFrequency = value; }
        public bool EnableCollapseBackward { get => enableCollapseBackward; private set => enableCollapseBackward = value; }
        public float CollapseAnimationSpeed { get => collapseAnimationSpeed; private set => collapseAnimationSpeed = value; }
        public List<SnakeBodyData> BodyDataQueue { get => bodyDataQueue; set => bodyDataQueue = value; }

        private Boss boss;
        public void Initialize(Boss boss)
        {
            this.boss = boss; 
            LoadBodyConfig();
        }

        private void LoadBodyConfig()
        {
            if (jsonConfig == null) return;
            SnakeConfig config = JsonUtility.FromJson<SnakeConfig>(jsonConfig.text);
            bodyDataQueue.AddRange(config.bodyParts);
        }
    }
}