using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton Interface for each scene handler's basic functionalities.
/// </summary>
public abstract class ISceneHandler : MonoBehaviour
{
    public static ISceneHandler Instance;

    public Vector3 GoBackPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.LogWarning("ATTENTION: " + this + " has been destroyed because of double singleton");
            Destroy(this.gameObject);
        }
    }

    public virtual void OnGameOver()
    {
        GameStateHandler.Instance.GameOver();
    }

    public virtual void OnUnpausableCutscene(bool playing)
    {
        if (playing)
        {
            GameStateHandler.Instance.UnpausableCutsceneStarted();
        }
        else
        {
            GameStateHandler.Instance.UnpausableCutsceneEnded();
        }
    }

    public void OnInteractingWithComputer(bool isInteracting)
    {
        GameStateHandler.Instance.InteractingWithComputer(isInteracting);
    }
}
