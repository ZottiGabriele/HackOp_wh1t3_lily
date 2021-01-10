using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using System;

public class OfficeEntranceSceneHandler : ISceneHandler
{
    [SerializeField] TerminalHandler _terminalHandler;
    [SerializeField] CinemachineTriggerAction _cameraFoundTrigger;
    [SerializeField] GameObject _securityCameraRay;
    [SerializeField] PlayableDirector _secondChallengeCompleted;

    private void Start()
    {
        _terminalHandler.OnChallengeCompleted += OnChallengeCompleted;

        _cameraFoundTrigger.enabled = !GameStateHandler.Instance.GameData.SecurityCameraFound;
        _securityCameraRay.SetActive(!GameStateHandler.Instance.GameData.SecondChallengeCompleted);
    }

    private void OnDestroy()
    {
        _terminalHandler.OnChallengeCompleted -= OnChallengeCompleted;
    }

    private void OnChallengeCompleted()
    {
        if (GameStateHandler.Instance.GameData.SecondChallengeCompleted) return;
        _secondChallengeCompleted.Play();
        GameStateHandler.Instance.GameData.SecondChallengeCompleted = true;
        GameStateHandler.Instance.SaveGame();
    }

    public void SecurityCameraFound()
    {
        GameStateHandler.Instance.GameData.SecurityCameraFound = true;
        GameStateHandler.Instance.SaveGame();
    }

    public void EnteredOffice()
    {
        GameStateHandler.Instance.GameData.EnteredOffice = true;
        GameStateHandler.Instance.SaveGame();
    }
}
