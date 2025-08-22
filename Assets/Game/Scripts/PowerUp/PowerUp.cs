using Game.Controllers;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Game.PowerUp
{
    public class PowerUp : MonoBehaviour
    {
        //[SerializeField] private PowerUpType powerUpType;
        [SerializeField] private TargetType targetType;

        private PowerUpEffect powerUpEffect;

        private void Start()
        {
            powerUpEffect = GetComponent<PowerUpEffect>();
            //powerUpEffect.enabled = true;
            //Debug.Log(powerUpEffect.ToString());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
            GameObject target = GetTarget(collision.gameObject);
            Debug.Log(target);
            if (target != null)
            {
                AddComponentToTarget(target);
            }
            Destroy(gameObject);
        }

        private GameObject GetTarget(GameObject player)
        {
            if (targetType == TargetType.Player) return player;
            if (targetType == TargetType.Weapon)
            {
                Transform child = player.transform.GetChild(0);
                return child.gameObject;
            }
            if (targetType == TargetType.Missile)
            {
                Transform child = player.transform.GetChild(0).GetChild(0);
                return child.gameObject;
            }
            return null;
        }

        //private float GetRatioByEV(float min, float max, float currentEV, float ratioMin, float ratioMax)
        //{
        //    int parts = 3;
        //    if (min >= max) return 0;

        //    currentEV = Math.Clamp(currentEV, min, max);

        //    float step = (max - min) / (float)parts;
        //    int index = (int)((currentEV - min) / step);
        //    index = Math.Clamp(index, 0, parts - 1);

        //    float ratioStep = (ratioMax - ratioMin) / (float)(parts - 1);
        //    return (float)(ratioMin + ratioStep * index);
        //}



        //private void AddComponentToTarget(GameObject target)
        //{
        //    Debug.Log($"Game.PowerUp.{powerUpType}");
        //    Type effectType = Type.GetType($"Game.PowerUp.{powerUpType}");
        //    if (effectType == null) return;
        //    target.AddComponent(effectType);
        //}

        private void AddComponentToTarget(GameObject target)
        {
            Debug.Log(target);
            Debug.Log(powerUpEffect.GetType().Name);
            Component newEffect = target.AddComponent(powerUpEffect.GetType());
            Debug.Log(newEffect.gameObject);
            if (newEffect != null)
            {
                Debug.Log("Efect not null");
                var json = JsonUtility.ToJson(powerUpEffect);
                JsonUtility.FromJsonOverwrite(json, newEffect);
            }
        }
    }

    public enum PowerUpType
    {
        //weapon
        RapidFire,
        DoubleShot,
        BurstShot,
        DiagonalFire,


        //missile
        Homing,
        BounceShot,
        PierceShot,
        PowerShot,
        DamageBoost,

        //player
        Invisible,
        Shield
    }

    public enum TargetType
    {
        Player,
        Weapon,
        Missile
    }
}