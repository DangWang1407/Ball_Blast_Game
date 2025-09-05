using Game.Events;

namespace Game.Currency
{
    public struct GoldChangedEvent : IGameEvent
    {
        public int NewValue { get; }
        public int Delta { get; }

        public GoldChangedEvent(int newValue, int delta)
        {
            NewValue = newValue;
            Delta = delta;
        }
    }
}

