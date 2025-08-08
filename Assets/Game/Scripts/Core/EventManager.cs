using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventManager
{
    private static Dictionary<Type, List<Delegate>> eventListeners = new Dictionary<Type, List<Delegate>>();

    public static void Subscribe<T>(Action<T> listener) where T : IGameEvent
    {
        Type eventType = typeof(T);
        if (!eventListeners.ContainsKey(eventType))
        {
            eventListeners[eventType] = new List<Delegate>();
        }

        eventListeners[eventType].Add(listener);
    }

    public static void Unsubscribe<T>(Action<T> listener) where T : IGameEvent
    {
        Type eventType = typeof(T);
        if (eventListeners.ContainsKey(eventType))
        {
            eventListeners[eventType].Remove(listener);
            if (eventListeners[eventType].Count == 0)
            {
                eventListeners.Remove(eventType);
            }
        }
    }

    public static void Trigger<T>(T gameEvent) where T : IGameEvent
    {
        Type eventType = typeof(T);
        if (eventListeners.ContainsKey(eventType))
        {
            foreach (Action<T> listener in eventListeners[eventType])
            {
                try
                {
                    listener?.Invoke(gameEvent);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error invoking listener for event {eventType.Name}: {ex.Message}");
                }
            }
        }
    }
}

public interface IGameEvent { }