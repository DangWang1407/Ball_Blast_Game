using Game.Scriptable;
using UnityEngine;

namespace Game.Controllers
{
    public class MissileStats : MonoBehaviour
    {
        private MissileController missileController;

        private float missileSpeed;     
        private float bulletScale;
        private int damage;
        private bool canHoming;
        private bool canBounce;
        private bool canPierce;

        [SerializeField] MissileData baseMissileData;

        public float MissileSpeed { get => missileSpeed; set => missileSpeed = value; }
        public float BulletScale { get => bulletScale; set => bulletScale = value; }
        public int Damage { get => damage; set => damage = value; }
        public bool CanHoming { get => canHoming; set => canHoming = value; }
        public bool CanBounce { get => canBounce; set => canBounce = value; }
        public bool CanPierce { get => canPierce; set => canPierce = value; }

        public void Initialize(MissileController missileController)
        {
            this.missileController = missileController;
            if (baseMissileData != null)
            {
                ResetData();
            }
        }

        public void ResetData()
        {
            missileSpeed = baseMissileData.missileSpeed;
            bulletScale = baseMissileData.bulletScale;
            damage = baseMissileData.damage;
            canHoming = baseMissileData.canHoming;
            canBounce = baseMissileData.canBounce;
            canPierce = baseMissileData.canPierce;
        }


    }
}