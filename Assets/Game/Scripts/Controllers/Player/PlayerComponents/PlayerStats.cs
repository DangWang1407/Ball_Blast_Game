using UnityEngine;

namespace Game.Controllers
{
    public class PLayerStats : MonoBehaviour
    {
        private PlayerController playerController;

        private float boundaryOffset;
        private float moveSpeed;
        private float smoothing;
        private bool shield;
        private bool isInvisible;

        public void Initialize(PlayerController playerController)
        {
            this.playerController = playerController;
        }
    }
}