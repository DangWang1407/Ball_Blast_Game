using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class LevelEditorTimelineDrag
    {
        // Handles dragging of timeline markers
        public void Handle(Rect rect, LevelEditorModel model, float secondsPerPixel, float total, ref bool isDraggingMarker, ref int draggingIndex)
        {
            var e = Event.current;
            if (isDraggingMarker && draggingIndex >= 0 && draggingIndex < model.Meteors.Count)
            {
                if (e.type == EventType.MouseDrag)
                {
                    float t = model.TimelineScroll + (e.mousePosition.x - rect.x) * secondsPerPixel;
                    t = Mathf.Clamp(t, 0f, total);
                    model.Meteors[draggingIndex].spawnTime = t;
                    LevelEditorUtils.ClampSpawnTimes(model);
                    e.Use();
                }
                else if (e.type == EventType.MouseUp)
                {
                    isDraggingMarker = false;
                    draggingIndex = -1;
                    e.Use();
                }
            }
        }
    }
}
