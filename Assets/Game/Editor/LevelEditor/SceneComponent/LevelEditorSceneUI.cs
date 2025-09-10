using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Game.Controllers;

namespace Game.Editor
{
    public class LevelEditorSceneGUI
    {
        private LevelEditorModel model;
        private Action requestRepaint;

        private LevelEditorSceneBackground background;
        private LevelEditorMeteorPreview preview;
        private LevelEditorSceneHandles handles;

        public void Attach(LevelEditorModel m, Action repaint)
        {
            model = m;
            requestRepaint = repaint;
            SceneView.duringSceneGui += OnSceneGUI;
            background??= new LevelEditorSceneBackground();
            preview??= new LevelEditorMeteorPreview();
            handles??= new LevelEditorSceneHandles();
        }

        public void Detach()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            background?.ClearBackground();
            preview?.ClearMeteors();
            model = null;
            requestRepaint = null;
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (model == null) return;
            Event e = Event.current;

            background.EnsureBackground();
            preview.EnsurePrefabs();
            preview.EnsureMeteorsRoot();
            preview.SyncMeteorInstances(model, i => IsVisibleByFilter(model, i));

            // Draw interactive handles only 
            handles.DrawSceneHandles(model, requestRepaint, i => IsVisibleByFilter(model, i));

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
            model.SelectedIndex = model.Meteors.Count - 1;
            requestRepaint?.Invoke();
            SceneView.RepaintAll();
        }
        
        private static bool IsVisibleByFilter(LevelEditorModel model, int index)
        {
            return model.CurrentViewFilter switch
            {
                ViewFilter.All => true,
                ViewFilter.Single => index == model.SelectedIndex && model.SelectedIndex >= 0 && model.SelectedIndex < model.Meteors.Count,
                ViewFilter.Group => model.SelectedSet != null && model.SelectedSet.Contains(index),
                _ => true
            };
        }
    }
}
