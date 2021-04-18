using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Interface for all game objects that have to be interactable via the mouse input.
/// 
/// NOTE: Implementing this will set the gameobject layer to "Interaction"
/// </summary>
public abstract class IInteractionArea : IGameDataObserver
{
    protected virtual void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Interaction");
    }

    public void OnInteraction()
    {
        if (GameStateHandler.Instance.CurrentGameState == GameStateHandler.GameState.Paused ||
           GameStateHandler.Instance.CurrentGameState == GameStateHandler.GameState.UnpausableCutscene)
        {
            return;
        }
        executeOnCondition();
    }
}
