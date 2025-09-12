using UnityEngine;
using Game.Services;

namespace Game.UI
{
    [DisallowMultipleComponent]
    public class MeteorHealthUIBinder : MonoBehaviour
    {
        [Header("UI Settings")] public Vector3 worldOffset = Vector3.zero;
        public bool hideWhenZero = true;

        WorldToUIFollower _ui;
        string _poolName;

        void OnEnable()
        {
            var pooling = HealthTextUIPooling.Instance;
            if (pooling == null)
            {
                Debug.LogError("HealthTextUIPooling not found. Add it to HUD Canvas and assign prefab.");
                return;
            }

            _poolName = pooling.poolName;

            var parent = pooling.hudCanvas ? pooling.hudCanvas.transform : null;
            var go = PoolManager.Instance.Spawn(_poolName, Vector3.zero, Quaternion.identity, parent);
            if (go == null)
            {
                Debug.LogError($"Pool '{_poolName}' not ready. Check HealthTextUIPooling.");
                return;
            }
            _ui = go.GetComponent<WorldToUIFollower>();
            if (_ui == null)
            {
                Debug.LogError("HealthTextUI prefab is missing WorldToUIFollower.");
                return;
            }
            if (_ui.canvas == null) _ui.canvas = pooling.hudCanvas ? pooling.hudCanvas : _ui.GetComponentInParent<Canvas>(true);
            if (_ui.cam == null)
            {
                _ui.cam = _ui.canvas && _ui.canvas.renderMode != RenderMode.ScreenSpaceOverlay ? _ui.canvas.worldCamera : Camera.main;
            }
            _ui.target = transform;
            _ui.worldOffset = worldOffset;
        }

        void OnDisable()
        {
            if (_ui != null && PoolManager.Instance != null && !string.IsNullOrEmpty(_poolName))
            {
                PoolManager.Instance.Despawn(_poolName, _ui.gameObject);
            }
            _ui = null;
        }

        public void SetHealth(int hp)
        {
            if (_ui == null) return;
            if (hideWhenZero && hp <= 0)
            {
                _ui.gameObject.SetActive(false);
                return;
            }

            if (!_ui.gameObject.activeSelf) _ui.gameObject.SetActive(true);
            _ui.SetText(hp.ToString());
        }
    }
}
