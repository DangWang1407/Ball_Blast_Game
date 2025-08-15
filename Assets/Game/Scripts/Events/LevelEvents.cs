using UnityEngine;

namespace Game.Events
{
    public struct LevelUpgradeEvent : IGameEvent
    {
        public int CurrentLevel;
        public LevelUpgradeEvent(int currentLevel) 
        { 
            CurrentLevel = currentLevel; 
        }
    }
}
