using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        [SerializeField] private int maxMarkers = 100;

        //public bool RecordingEnabled = true;

        // Cached array for efficient offset access
        private Marker[] markerArray;
        private bool arrayDirty = true;

        private void FixedUpdate()
        {
            //if (!RecordingEnabled) return;
            AddMarker(transform.position, transform.rotation);
        }

        public void AddMarker(Vector3 position, Quaternion rotation)
        {
            markerQueue.Enqueue(new Marker(position, rotation));

            if (markerQueue.Count > maxMarkers)
                markerQueue.Dequeue();

            arrayDirty = true; 
        }

        public Marker GetNextMarker()
        {
            if (markerQueue.Count > 0)
            {
                arrayDirty = true;
                return markerQueue.Dequeue();
            }
            return null;
        }

        public Marker GetMarkerAtOffset(int offset)
        {
            if (markerQueue.Count == 0 || offset < 0) return null;

            if (arrayDirty)
            {
                markerArray = markerQueue.ToArray();
                arrayDirty = false;
            }

            // Return marker at offset (0 = newest, higher = older)
            int index = markerArray.Length - 1 - offset;
            return (index >= 0 && index < markerArray.Length) ? markerArray[index] : null;
        }

        public bool HasMarkers()
        {
            return markerQueue.Count > 0;
        }

        public int MarkerCount => markerQueue.Count;

        public void ClearMarkers()
        {
            markerQueue.Clear();
            AddMarker(transform.position, transform.rotation);
            arrayDirty = true;
        }

        public void SetMaxMarkers(int newMax)
        {
            maxMarkers = Mathf.Max(1, newMax);

            while (markerQueue.Count > maxMarkers)
            {
                markerQueue.Dequeue();
            }

            arrayDirty = true;
        }

    }
}