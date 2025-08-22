using Game.Events;
using Game.PowerUp;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Controllers
{
    public class MissileEffectList : MonoBehaviour
    {
        private WeaponController weaponController;

        public void Initialize(WeaponController weaponController)
        {
            this.weaponController = weaponController;
        }

        public void ApplyEffect(GameObject obj)
        {
            Transform child = gameObject.transform.GetChild(0);
            var effects = child.gameObject.GetComponents<PowerUpEffect>();

            foreach (var effect in effects)
            {
                var existingEffect = obj.GetComponent(effect.GetType()) as PowerUpEffect;
                PowerUpEffect targetEffect;

                if (existingEffect == null)
                {
                    targetEffect = obj.AddComponent(effect.GetType()) as PowerUpEffect;
                }
                else
                {
                    targetEffect = existingEffect;
                }

                var json = JsonUtility.ToJson(effect);
                JsonUtility.FromJsonOverwrite(json, targetEffect);

            }
        }

    }
}