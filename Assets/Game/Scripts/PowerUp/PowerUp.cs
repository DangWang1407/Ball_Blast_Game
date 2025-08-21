using Game.Controllers;
using System;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Game.PowerUp
{
    public class PowerUp : MonoBehaviour
    {
        [SerializeField] private PowerUpType powerUpType;
        [SerializeField] private TargetType targetType;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
            GameObject target = GetTarget(collision.gameObject);
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

        private void AddComponentToTarget(GameObject target)
        {
            Debug.Log($"Game.PowerUp.{powerUpType}");
            Type effectType = Type.GetType($"Game.PowerUp.{powerUpType}");
            if (effectType == null) return;
            target.AddComponent(effectType);
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