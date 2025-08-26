using Game.Scriptable;
using UnityEngine;

namespace Game.Controllers
{
    public class MissileStats : MonoBehaviour
    {
        private MissileController missileController;

        private float missileSpeed = 6f;
        private float bulletScale = 0.08f;
        private int damage = 1;
        private bool canBounce = false;
        private bool canPierce = false;

        // [SerializeField] MissileData baseMissileData;

        public float MissileSpeed { get => missileSpeed; set => missileSpeed = value; }
        public float BulletScale { get => bulletScale; set => bulletScale = value; }
        public int Damage { get => damage; set => damage = value; }
        public bool CanBounce { get => canBounce; set => canBounce = value; }
        public bool CanPierce { get => canPierce; set => canPierce = value; }

        public void Initialize(MissileController missileController)
        {
            this.missileController = missileController;
        }
    }
}