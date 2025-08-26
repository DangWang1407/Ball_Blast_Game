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

        [SerializeField] private Queue<Marker> markerQueue = new Queue<Marker>();
        private int maxMarkers = 50;
        public bool RecordingEnabled = true;

        private void FixedUpdate()
        {
            if (!RecordingEnabled) return;
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


        // >>> NEW: clone/replace/peek-last giúp hoán đổi queue
        public Queue<Marker> CloneQueue() => new Queue<Marker>(markerQueue);

        public void ReplaceQueue(Queue<Marker> newQueue)
        {
            markerQueue = new Queue<Marker>(newQueue);
        }

        public Marker PeekLast()
        {
            if (markerQueue.Count == 0) return null;
            Marker last = null;
            foreach (var m in markerQueue) last = m;
            return last;
        }

        public void ClearAndSeed(Vector3 pos, Quaternion rot)
        {
            markerQueue.Clear();
            AddMarker(pos, rot);
        }
    }
}