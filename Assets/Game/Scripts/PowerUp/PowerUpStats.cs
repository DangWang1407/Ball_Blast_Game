using Unity;
using UnityEngine;
namespace Game.PowerUp
{
    public abstract class PowerUpStats : MonoBehaviour
    {
        protected float baseDuration = 20f;

        virtual public float GetDuration(int level) => baseDuration;
    }
}