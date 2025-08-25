using System.Collections.Generic;
using UnityEngine;

namespace Game.Controllers
{
    public class MarkerManager : MonoBehaviour
    {
        public class Marker
        {
            public Vector3 Position;
            public Quaternion Rotation;

            public Marker(Vector3 position, Quaternion rotation)
            {
                Position = position;
                Rotation = rotation;
            }
        }

        private Queue<Marker> markerQueue = new Queue<Marker>();
        private int maxMarkers = 50; 

        private void FixedUpdate()
        {
            AddMarker(transform.position, transform.rotation);
        }

        public void AddMarker(Vector3 position, Quaternion rotation)
        {
            markerQueue.Enqueue(new Marker(position, rotation));

            if (markerQueue.Count > maxMarkers)
                markerQueue.Dequeue();
        }

        public Marker GetNextMarker()
        {
            return markerQueue.Count > 0 ? markerQueue.Dequeue() : null;
        }

        public bool HasMarkers()
        {
            return markerQueue.Count > 0;
        }

        public void ClearMarkers()
        {
            markerQueue.Clear();
            AddMarker(transform.position, transform.rotation);
        }
    }
}
