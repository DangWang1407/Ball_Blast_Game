using System;
using UnityEditor;
using UnityEngine;
using Game.Controllers;

namespace Game.Editor
{
    public class LevelEditorSceneGUI
    {
        private LevelEditorModel model;
        private Action requestRepaint;

        private const string BackgroundPrefabPath = "Assets/Game/Prefabs/Background/Background.prefab";
        private GameObject backgroundGo;

        public void Attach(LevelEditorModel m, Action repaint)
        {
            model = m;
            requestRepaint = repaint;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        public void Detach()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            ClearBackground();
            model = null;
            requestRepaint = null;
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (model == null) return;
            Event e = Event.current;

            EnsureBackground();
            if (backgroundGo == null)
            {
                DrawGrid(Vector3.zero, 1f, 20);
            }

            // Draw interactive handles only (no meteor prefab previews)
            DrawSceneHandles();

            if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
            {
                if (TryGetMouseWorldOnPlane(e.mousePosition, out var world))
                {
                    if (LevelEditorUtils.IsCreateTool(model.CurrentTool))
                    {
                        AddMeteorFromClick(world);
                        e.Use();
                    }
                }
            }
        }

        private void DrawSceneHandles()
        {
            for (int i = 0; i < model.Meteors.Count; i++)
            {
                var m = model.Meteors[i];
                float r = m.size switch { MeteorSize.Large => 0.6f, MeteorSize.Medium => 0.45f, _ => 0.3f };

                Handles.color = i == model.SelectedIndex ? new Color(0.3f, 0.8f, 1f, 0.9f) : new Color(1f, 1f, 1f, 0.5f);
                Handles.DrawWireDisc(m.position, Vector3.forward, r);

                Handles.color = new Color(0,0,0,0);
                Vector3 handlePos = Handles.FreeMoveHandle(m.position, r * 0.8f, Vector3.one * 0.1f, Handles.CircleHandleCap);
                if (handlePos != m.position)
                {
                    m.position = new Vector3(handlePos.x, handlePos.y, 0f);
                    requestRepaint?.Invoke();
                }
                // Small label for timing and index
                Handles.color = Color.white;
                GUIStyle style = new GUIStyle(EditorStyles.miniBoldLabel) { normal = { textColor = Color.white } };
                Handles.Label(m.position + new Vector3(0.1f, 0.1f, 0), $"{i + 1}\n{m.spawnTime:0.0}s", style);
            }
        }

        private void EnsureBackground()
        {
            if (backgroundGo != null) return;
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(BackgroundPrefabPath);
            if (prefab == null) return;
            var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (go == null) return;
            go.name = "LevelEditorBackground";
            go.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
            var pos = go.transform.position;
            go.transform.position = new Vector3(pos.x, pos.y, 1f); // behind meteors at z=0
            foreach (var mb in go.GetComponentsInChildren<MonoBehaviour>()) mb.enabled = false;
            backgroundGo = go;
        }

        private void ClearBackground()
        {
            if (backgroundGo != null)
            {
                UnityEngine.Object.DestroyImmediate(backgroundGo);
                backgroundGo = null;
            }
        }

        private void DrawGrid(Vector3 origin, float spacing, int halfCount)
        {
            Handles.color = new Color(1f, 1f, 1f, 0.08f);
            float min = -halfCount * spacing;
            float max = halfCount * spacing;
            for (int i = -halfCount; i <= halfCount; i++)
            {
                float x = i * spacing + origin.x;
                float y = i * spacing + origin.y;
                Handles.DrawLine(new Vector3(x, min + origin.y, 1f), new Vector3(x, max + origin.y, 1f));
                Handles.DrawLine(new Vector3(min + origin.x, y, 1f), new Vector3(max + origin.x, y, 1f));
            }
            Handles.color = new Color(1f, 1f, 1f, 0.2f);
            Handles.DrawLine(new Vector3(min + origin.x, origin.y, 1f), new Vector3(max + origin.x, origin.y, 1f));
            Handles.DrawLine(new Vector3(origin.x, min + origin.y, 1f), new Vector3(origin.x, max + origin.y, 1f));
        }

        private bool TryGetMouseWorldOnPlane(Vector2 mousePos, out Vector3 world)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);
            Plane plane = new Plane(Vector3.forward, Vector3.zero);
            if (plane.Raycast(ray, out float dist))
            {
                world = ray.GetPoint(dist);
                world.z = 0f;
                return true;
            }
            world = Vector3.zero;
            return false;
        }
        private void AddMeteorFromClick(Vector3 position)
        {
            var data = LevelEditorUtils.CreateMeteor(position, model.CurrentTool, 0f);
            model.Meteors.Add(data);
            requestRepaint?.Invoke();
            SceneView.RepaintAll();
        }
    }
}