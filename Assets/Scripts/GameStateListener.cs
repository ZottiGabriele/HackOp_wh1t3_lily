using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class GameStateListener : MonoBehaviour
{
    [SerializeField] List<GameStateEvent> events = new List<GameStateEvent>();

    void Start()
    {
        GameStateHandler.Instance.OnGameStateChanged += onGameStateChanged;
    }

    private void OnDestroy()
    {
        GameStateHandler.Instance.OnGameStateChanged -= onGameStateChanged;
    }

    private void onGameStateChanged(GameStateHandler.GameState state)
    {
        foreach (var e in events)
        {
            if (e.targetState == state)
            {
                e.callback.Invoke();
            }
        }
    }
}

[System.Serializable]
public struct GameStateEvent
{
    public GameStateHandler.GameState targetState;
    public UnityEvent callback;
}
