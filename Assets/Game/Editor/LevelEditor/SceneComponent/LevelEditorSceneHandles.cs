using UnityEditor;
using UnityEngine;
using Game.Controllers;

namespace Game.Editor
{
    public class LevelEditorSceneHandles
    {
        public void DrawSceneHandles(LevelEditorModel model, System.Action requestRepaint, System.Func<int, bool> isVisible)
        {
            for (int i = 0; i < model.Meteors.Count; i++)
            {
                if (isVisible != null && !isVisible(i)) continue;
                var m = model.Meteors[i];
                float r = m.size switch { MeteorSize.Large => 1f, MeteorSize.Medium => 0.75f, _ => 0.5f };

                bool isPrimary = i == model.SelectedIndex;
                if (isPrimary)
                {
                    Handles.color = Color.cyan;
                    Handles.DrawWireDisc(m.position, Vector3.forward, r);
                }

                Handles.color = new Color(0, 0, 0, 0);
                Vector3 handlePos = Handles.FreeMoveHandle(m.position, r * 0.8f, Vector3.one * 0.1f, Handles.CircleHandleCap);
                if (handlePos != m.position)
                {
                    model.SelectedIndex = i;
                    m.position = new Vector3(handlePos.x, handlePos.y, 0f);
                    requestRepaint?.Invoke();
                }

                Handles.color = Color.white;
                GUIStyle style = new GUIStyle(EditorStyles.miniBoldLabel) { normal = { textColor = Color.white } };
                Handles.Label(m.position + new Vector3(0.1f, 0.1f, 0), $"{i + 1}\n{m.spawnTime:0.0}s", style);
            }
        }
    }
}

