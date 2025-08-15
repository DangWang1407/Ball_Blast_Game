using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
    public class ChaseableEntity : MonoBehaviour
    {
        public static readonly HashSet<ChaseableEntity> AllEntities = new HashSet<ChaseableEntity>();

        private void OnEnable()
        {
            AllEntities.Add(this);
        }

        void OnDisable()
        {
            AllEntities.Remove(this);
        }

        public static void ClearAllEntities()
        {
            AllEntities.Clear();
        }
    }
}