using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
    public class ChaseableEntity : MonoBehaviour
    {
        public static readonly HashSet<ChaseableEntity> AllEntities = new HashSet<ChaseableEntity>();

        void Awake()
        {
            AllEntities.Add(this);
        }

        void OnDestroy()
        {
            AllEntities.Remove(this);
        }
    }
}