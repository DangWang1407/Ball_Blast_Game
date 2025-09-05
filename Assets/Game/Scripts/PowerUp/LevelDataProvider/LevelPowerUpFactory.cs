using System.Collections.Generic;
using UnityEngine;

namespace Game.PowerUp
{
    public class LevelPowerUpFactory 
    {
        public static Dictionary<PowerUpType, int> GetData(ProviderType type)
        {
            Dictionary<PowerUpType, int> data = null;
            switch (type)
            {
                case ProviderType.Default:
                    break;
                case ProviderType.Manager:
                    data = LevelPowerUpManager.Instance.GetLevelPowerUps();
                    break;
                case ProviderType.Server:
                    
                    break;
            }
            return data;
        }
    }

    public enum ProviderType
    {
        Default,
        Manager,
        Server
    }
}