using Game.Events;
using UnityEngine;

namespace Game.Controllers
{
    public class MeteorSpawnerSplitting : MonoBehaviour
    {
        private MeteorSpawnerController controller;
        private MeteorSpawnerPooling spawnerPooling;
        private readonly float[] directions = { -1f, 1f };

        private void Start()
        {
            EventManager.Subscribe<SplitMeteorEvent>(OnSplitMeteor);
        }

        public void Initialize(MeteorSpawnerController controller)
        {
            this.controller = controller;
            spawnerPooling = GetComponent<MeteorSpawnerPooling>();
        }

        private void OnSplitMeteor(SplitMeteorEvent e)
        {
            SpawnSplitMeteors(e.Position, e.MeteorSize);
        } 

        public void SpawnSplitMeteors(Vector3 position, MeteorSize currentSize)
        {
            if (currentSize == MeteorSize.Small) return;

            MeteorSize newSize = currentSize == MeteorSize.Large ? MeteorSize.Medium : MeteorSize.Small;

            for (int i = 0; i < 2; i++)
            {
                GameObject split = spawnerPooling.SpawnMeteor((int)newSize, position);
                if (split != null)
                {
                    split.GetComponent<MeteorController>().Initialize(spawnerPooling.PoolNames[(int)newSize]);
                    split.GetComponent<MeteorController>().SetMeteorSize(newSize);
                    split.GetComponent<Rigidbody2D>().velocity = new Vector2(directions[i] * 3f, 5f);
                }
            }
        }

        private void OnDestroy()
        {
            EventManager.Unsubscribe<SplitMeteorEvent>(OnSplitMeteor);
        }
    }
}