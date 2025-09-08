using UnityEngine;

namespace Game.Controllers
{
    public class MeteorMovement  : MonoBehaviour
    {
        [SerializeField] private float angularVelocity = 10f;

        private MeteorController meteorController;

        public void Initialize(MeteorController meteorController)
        {
            this.meteorController = meteorController;
        }

        public void UpdateRotation()
        {
            // meteorController.Rigidbody.angularDrag = angularVelocity;
        }

        public void ResetMovement()
        {
            meteorController.Rigidbody.mass = 1f;
            // meteorController.Rigidbody.drag = 0.2f;
        }
    }
}
