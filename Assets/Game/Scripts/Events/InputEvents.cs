using System;
using UnityEngine;


namespace Game.Events
{
    public struct PlayerInputEvent : IGameEvent
    {
        public InputType Type { get; }
        public Vector2 Position { get; }

        public PlayerInputEvent(InputType type, Vector2 position)
        {
            Type = type; Position = position;
        }

    }

    public enum InputType
    {
        TouchStart,
        TouchMove,
        TouchEnd,
        Fire,
        Pause
    }
}