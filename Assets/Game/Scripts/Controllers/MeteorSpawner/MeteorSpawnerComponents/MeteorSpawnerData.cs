using UnityEngine;
namespace Game.Controllers
{
    public class MeteorSpawnerData : MonoBehaviour
    {
        private MeteorSpawnerController controller;
        private TextAsset meteorsJson;
        private MeteorSpawnList meteors;

        public MeteorSpawnList Meteors => meteors;

        public void Initialize(MeteorSpawnerController controller)
        {
            this.controller = controller;
        }

        public void LoadMeteorData(TextAsset levelData)
        {
            meteorsJson = levelData;

            if (meteorsJson == null)
            {
                Debug.Log("Meteor json is null");
                return;
            }

            meteors = JsonUtility.FromJson<MeteorSpawnList>(meteorsJson.text);
            Debug.Log("Meteor data loaded successfully");
        }
    }


    [System.Serializable]
    public class MeteorData
    {
        public float spawnTime;
        public Vector3 position;
        public MeteorSize size;

    }

    [System.Serializable]
    public class MeteorSpawnList
    {
        public MeteorData[] meteors;
    }
}
