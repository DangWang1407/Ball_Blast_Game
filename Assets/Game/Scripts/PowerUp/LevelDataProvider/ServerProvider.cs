using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PowerUp
{
    public class ServerProvider : MonoBehaviour 
    {
        private Dictionary<PowerUpType, int> levelPowerUps = new Dictionary<PowerUpType, int>();
        public Dictionary<PowerUpType, int> GetlevelData() 
        { 
            return levelPowerUps; 
        }

    }
}