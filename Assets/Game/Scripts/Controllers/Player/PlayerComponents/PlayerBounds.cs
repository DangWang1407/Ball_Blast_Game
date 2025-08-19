using Game.Core;
using UnityEngine;

namespace Game.Controllers
{
    public class PlayerBounds : MonoBehaviour
    {
        [SerializeField] private float boundaryOffset = 0.56f;

        private PlayerController playerController;
        private float screenBounds;

        public void Initialize(PlayerController playerController)
        {
            this.playerController = playerController;
            CalculateScreenBounds();
        }

        private void CalculateScreenBounds()
        {
            if (playerController.Camera != null)
                screenBounds = playerController.Camera.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x - boundaryOffset;
            else
                screenBounds = GameManager.Instance.ScreenWidth - boundaryOffset;
        }

        public Vector2 ClampPosition(Vector2 position)
        {
            position.x = Mathf.Clamp(position.x, -screenBounds, screenBounds);
            return position;
        }

        public void OnDestroy()
        {
            
        }
    }
}