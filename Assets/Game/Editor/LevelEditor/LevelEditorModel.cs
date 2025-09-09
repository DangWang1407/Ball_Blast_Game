using System.Collections.Generic;
using Game.Controllers;

namespace Game.Editor
{
    public enum ToolMode
    {
        None,
        CreateLarge,
        CreateMedium,
        CreateSmall
    }

    [System.Serializable]
    public class LevelEditorModel
    {
        public float Duration = 30f;
        public string CurrentAssetPath = string.Empty;
        public ToolMode CurrentTool = ToolMode.None;

        [UnityEngine.SerializeField]
        public List<MeteorData> Meteors = new List<MeteorData>();

        public int SelectedIndex = -1;
        public float TimelineZoom = 1f;
        public float TimelineScroll = 0f;
    }
}