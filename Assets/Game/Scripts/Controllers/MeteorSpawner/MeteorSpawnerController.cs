using System.Collections;
using UnityEngine;
using Game.Services;
using Game.Core;
using Game.Events;

namespace Game.Controllers
{
    public class MeteorSpawnerController : MonoBehaviour
    {
        [SerializeField] private GameObject[] meteorPrefabs; // Large, Medium, Small
        [SerializeField] private int meteorsCount = 12;
        [SerializeField] private float spawnDelay = 4f;

        public GameObject[] MeteorPrefabs => meteorPrefabs;
        public int MeteorsCount => meteorsCount;
        public float SpawnDelay => spawnDelay;

        // Components
        private MeteorSpawnerPooling spawnerPooling;
        private MeteorSpawnerData spawnerData;
        private MeteorSpawnerSpawning spawnerSpawning;
        private MeteorSpawnerSplitting spawnerSplitting;

        //public static MeteorSpawnerController Instance { get; private set; }

        private void Awake()
        {
            //Instance = this;
            Initialize();
        }

        private void Initialize()
        {
            spawnerPooling = GetComponent<MeteorSpawnerPooling>();
            spawnerData = GetComponent<MeteorSpawnerData>();
            spawnerSpawning = GetComponent<MeteorSpawnerSpawning>();
            spawnerSplitting = GetComponent<MeteorSpawnerSplitting>();

            //spawnerPooling = gameObject.AddComponent<MeteorSpawnerPooling>();
            //spawnerSpawning = gameObject.AddComponent<MeteorSpawnerSpawning>();
            //spawnerSplitting = gameObject.AddComponent<MeteorSpawnerSplitting>();
            //spawnerData = gameObject.AddComponent<MeteorSpawnerData>();

            spawnerPooling.Initialize(this);
            spawnerData.Initialize(this);
            spawnerSpawning.Initialize(this);
            spawnerSplitting.Initialize(this);

            EventManager.Subscribe<LevelStartEvent>(OnLevelStart);
        }

        private void Start()
        {
            spawnerPooling.CreatePools();
        }

        private void OnLevelStart(LevelStartEvent ev)
        {
            spawnerData.LoadMeteorData(ev.LevelData);
            spawnerSpawning.StartSpawning();
        }

        private void OnDestroy()
        {
            EventManager.Unsubscribe<LevelStartEvent>(OnLevelStart);
        }
    }
}
