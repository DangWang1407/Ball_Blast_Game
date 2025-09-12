using TMPro;
using UnityEngine;

namespace Game.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public class WorldToUIFollower : MonoBehaviour
    {
        [Header("Follow Target")]
        public Transform target;
        public Vector3 worldOffset = Vector3.zero;

        [Header("References")]
        public TextMeshProUGUI label;
        public Canvas canvas;
        public Camera cam;

        RectTransform rect;
        RectTransform canvasRect;

        void Awake()
        {
            rect = (RectTransform)transform;
            if (canvas == null) canvas = GetComponentInParent<Canvas>(true);
            if (canvas != null) canvasRect = (RectTransform)canvas.transform;
            if (cam == null)
            {
                cam = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay
                    ? canvas.worldCamera
                    : Camera.main;
            }
        }

        void LateUpdate()
        {
            if (target == null || canvas == null) return;

            // Convert world position to screen point
            var wp = target.position + worldOffset;
            var sp = (cam != null ? cam : Camera.main).WorldToScreenPoint(wp);

            // Hide when behind camera
            bool visible = sp.z > 0f;
            if (!visible)
            {
                if (label != null) label.enabled = false;
                else gameObject.SetActive(false);
                return;
            }

            if (canvasRect == null) canvasRect = (RectTransform)canvas.transform;
            var camForCanvas = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, sp, camForCanvas, out var lp);
            rect.anchoredPosition = lp;

            if (label != null && !label.enabled) label.enabled = true;
        }

        public void SetText(string t)
        {
            if (label != null) label.text = t;
        }
    }
}
