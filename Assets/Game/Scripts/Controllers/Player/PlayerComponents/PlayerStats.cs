using UnityEngine;

namespace Game.Controllers
{
    public class PlayerStats : MonoBehaviour
    {
        private PlayerController playerController;

        // Add properties với getter/setter
        private float boundaryOffset = 0.5f;
        private float moveSpeed = 5f;
        private float smoothing = 10f;
        private bool shield = false;
        private bool isInvisible = false;

        public float BoundaryOffset { get => boundaryOffset; set => boundaryOffset = value; }
        public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
        public float Smoothing { get => smoothing; set => smoothing = value; }
        public bool Shield { get => shield; set => shield = value; }
        public bool IsInvisible { get => isInvisible; set => isInvisible = value; }
        public void Initialize(PlayerController playerController)
        {
            this.playerController = playerController;
        }
    }
}