using System;
using UnityEngine;
using Game.Currency;
using Game.Events;

namespace Game.Currency
{
    public class GoldManager : MonoBehaviour
    {
        public static GoldManager Instance { get; private set; }

        [SerializeField] private int startingGold = 500;
        public int Gold { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Gold = Mathf.Max(0, startingGold);
            NotifyChanged(0);
        }

        public void Add(int amount)
        {
            if (amount <= 0) return;
            Gold += amount;
            NotifyChanged(+amount);
        }

        public bool CanSpend(int amount) => amount >= 0 && Gold >= amount;

        public bool Spend(int amount)
        {
            if (!CanSpend(amount)) return false;
            Gold -= amount;
            NotifyChanged(-amount);
            return true;
        }

        private void NotifyChanged(int delta)
        {
            EventManager.Trigger(new GoldChangedEvent(Gold, delta));
        }
    }
}
