using UnityEngine;

namespace Game.Controllers
{
    public class MissileHoming : MonoBehaviour
    {
        private MissileController missileController;
        private MissileMovement missileMovement;

        public void Initialize(MissileController missileController)
        {
            this.missileController = missileController;
            missileMovement = GetComponent<MissileMovement>();
        }


    }
}