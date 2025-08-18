using Game.Events;
using UnityEngine;

public class DebugClass : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Subcribe in debug level");
        EventManager.Subscribe<LevelStartEvent>(OnLevelStart);
    }

    void OnLevelStart(LevelStartEvent levelStartEvent)
    {
        Debug.Log("Level start in debug");
    }
}