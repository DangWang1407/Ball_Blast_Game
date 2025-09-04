using Game.Events;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Controllers
{
    public class PlayerInput : MonoBehaviour
    {
        private PlayerController playerController;
        private PlayerMovement playerMovement;
        public void Initialize(PlayerController playerController)
        {
            this.playerController = playerController;
            playerMovement = GetComponent<PlayerMovement>();
            
            EventManager.Subscribe<PlayerInputEvent>(OnPlayerInput);
        }

        public void OnUpdate()
        {
            HandleLegacyInput();
        }

        private void OnPlayerInput(PlayerInputEvent e)
        {
            switch (e.Type)
            {
                case InputType.TouchStart:
                case InputType.TouchMove:
                    playerMovement.TargetPosition = e.Position;
                    break;
            }
        }

        private void HandleLegacyInput()
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 mouseWorldPos = playerController.Camera.ScreenToWorldPoint(Input.mousePosition);
                playerMovement.TargetPosition = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
            }
        }

        public void OnDestroyPlayerInput()
        {
            EventManager.Unsubscribe<PlayerInputEvent>(OnPlayerInput);
        }
    }
}