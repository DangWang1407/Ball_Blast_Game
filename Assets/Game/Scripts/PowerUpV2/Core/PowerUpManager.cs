using System;
using System.Collections.Generic;
using UnityEngine;
using Game.PowerUp;
using Game.Events;
using Game.Controllers;

namespace Game.PowerUpV2
{
    public class PowerUpManager : MonoBehaviour
    {
        private LevelPowerUp levelPowerUp;
        [Serializable]
        public class ActivePowerUpState
        {
            public PowerUpDefinition definition;
            public int level;
            public float expireAt;
        }

        private Dictionary<PowerUpType, ActivePowerUpState> active = new();

        public void Activate(PowerUpDefinition definition, int level = 1)
        {
            if (definition == null) return;
            if (level < 1)
            {
                levelPowerUp ??= GetComponent<LevelPowerUp>();
                if (levelPowerUp != null)
                {
                    level = levelPowerUp.GetLevel(definition.Type);
                }
            }
            if (level < 1) level = 1;

            float duration = definition.GetDuration(level);
            var state = new ActivePowerUpState
            {
                definition = definition,
                level = level,
                expireAt = Time.time + duration
            };

            active[definition.Type] = state;

            // EventManager.Trigger(new PowerUpV2ActivatedEvent(definition.Type, definition, level));

            if (definition is IPlayerApplier playerApplier)
            {
                playerApplier.ApplyToPlayer(gameObject, level);
            }

            if (definition is IWeaponApplier weaponApplier)
            {
                var weapon = GetComponentInChildren<WeaponController>(true);
                if (weapon != null)
                {
                    weaponApplier.ApplyToWeapon(weapon.gameObject, level);
                }
            }
        }

        public bool IsActive(PowerUpType type) => active.ContainsKey(type) && active[type].expireAt > Time.time;

        public bool TryGetState(PowerUpType type, out ActivePowerUpState state)
        {
            if (active.TryGetValue(type, out state))
            {
                if (state.expireAt > Time.time) return true;
            }
            state = null;
            return false;
        }

        // public int GetLevel(PowerUpType type)
        // {
        //     return TryGetState(type, out var s) ? s.level : 1;
        // }

        private void Update()
        {
            // expire outdated
            var expired = new List<PowerUpType>();
            foreach (var kv in active)
            {
                if (kv.Value.expireAt <= Time.time)
                {
                    expired.Add(kv.Key);
                }
            }
            foreach (var t in expired)
            {
                if (active.TryGetValue(t, out var state))
                {
                    EventManager.Trigger(new PowerUpExpiredEvent(t, state.definition, state.level));

                    if (state.definition is IPlayerApplier playerApplier)
                    {
                        playerApplier.RemoveFromPlayer(gameObject);
                    }

                    if (state.definition is IWeaponApplier weaponApplier)
                    {
                        var weapon = GetComponentInChildren<WeaponController>(true);
                        if (weapon != null)
                        {
                            weaponApplier.RemoveFromWeapon(weapon.gameObject);
                        }
                    }
                }
                active.Remove(t);
            }
        }

        public IEnumerable<ActivePowerUpState> GetActiveStates()
        {
            foreach (var kv in active)
            {
                if (kv.Value.expireAt > Time.time)
                {
                    yield return kv.Value;
                }
            }
        }
    }
}
