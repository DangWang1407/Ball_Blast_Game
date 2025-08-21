using UnityEngine;
using Game.Controllers;
using System.Linq;

namespace Game.PowerUps
{
    public class PowerUpPickupController : MonoBehaviour
    {
        [SerializeField] private PowerUpEffectComponent[] effectComponents;
        [SerializeField] private PowerUpTarget targetType = PowerUpTarget.Player;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
            
            ApplyEffects(collision.gameObject);
            Destroy(gameObject);
        }
        
        private void ApplyEffects(GameObject player)
        {
            GameObject target = GetTarget(player);
            Debug.Log($"Applying effects to target: {target?.name}");
            if (target == null) return;
            
            foreach (var effectComponent in effectComponents)
            {
                if (effectComponent != null)
                {
                    CopyEffectToTarget(effectComponent, target);
                }
            }
        }
        
        private GameObject GetTarget(GameObject player)
        {
            Transform child = player.transform.GetChild(0);
            if (targetType == PowerUpTarget.Weapon) Debug.Log(targetType);
            return targetType switch
            {
                PowerUpTarget.Player => player,
                PowerUpTarget.Weapon => child.gameObject,
                _ => player
            };
        }
        
        private void CopyEffectToTarget(PowerUpEffectComponent source, GameObject target)
        {
            var newEffect = target.AddComponent(source.GetType()) as PowerUpEffectComponent;
            if (newEffect != null)
            {
                var json = JsonUtility.ToJson(source);
                JsonUtility.FromJsonOverwrite(json, newEffect);
            }
        }
    }
    
    public enum PowerUpTarget
    {
        Player,
        Weapon
    }
}