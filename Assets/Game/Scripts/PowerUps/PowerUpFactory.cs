using UnityEngine;
using System.Linq;

namespace Game.PowerUps
{
    public static class PowerUpFactory
    {
        public static void ApplyStoredEffectsToMissile(GameObject missile, GameObject weapon)
        {
            var missileEffects = weapon.GetComponents<PowerUpEffectComponent>()
                .Where(effect => effect.GetType().Namespace.Contains("Missile"));
            
            foreach (var effect in missileEffects)
            {
                var newEffect = missile.AddComponent(effect.GetType()) as PowerUpEffectComponent;
                if (newEffect != null)
                {
                    var json = JsonUtility.ToJson(effect);
                    JsonUtility.FromJsonOverwrite(json, newEffect);
                }
            }
        }
    }
}
