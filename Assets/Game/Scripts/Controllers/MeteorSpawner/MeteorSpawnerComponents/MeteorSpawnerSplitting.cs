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
            SpawnSplitMeteors(e.Position, e.MeteorSize, e.ChildHealth);
        } 

        public void SpawnSplitMeteors(Vector3 position, MeteorSize currentSize, int childHealth)
        {
            if (currentSize == MeteorSize.Small) return;

            MeteorSize newSize = currentSize == MeteorSize.Large ? MeteorSize.Medium : MeteorSize.Small;

            for (int i = 0; i < 2; i++)
            {
                GameObject split = spawnerPooling.SpawnMeteor((int)newSize, position);
                if (split != null)
                {
                    var splitController = split.GetComponent<MeteorController>();
                    splitController.Initialize(spawnerPooling.PoolNames[(int)newSize]);
                    splitController.SetMeteorSize(newSize);
                    if (childHealth > 0)
                    {
                        splitController.SetMeteorHealth(childHealth);
                    }
                    var rb = split.GetComponent<Rigidbody2D>();
                    rb.velocity = new Vector2(directions[i] * 1f, 5f);
                    rb.gravityScale = 1f;
                    rb.AddTorque(Random.Range(-20f, 20f));
                }
            }
        }

        private void OnDestroy()
        {
            EventManager.Unsubscribe<SplitMeteorEvent>(OnSplitMeteor);
        }
    }
}
