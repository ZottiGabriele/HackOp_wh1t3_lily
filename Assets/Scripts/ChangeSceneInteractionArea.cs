using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PolygonCollider2D))]
public class ChangeSceneInteractionArea : IInteractionArea
{
    [SerializeField] [HideInInspector] int _sceneToLoad;
    [SerializeField] Cutscene _fadeout;

    protected override void execute()
    {
        SoundsHandler.Instance.PlayDoorSound();
        _fadeout.ForcePlay(() =>
        {
            GameStateHandler.Instance.ChangeScene(_sceneToLoad);
        });
    }
}
