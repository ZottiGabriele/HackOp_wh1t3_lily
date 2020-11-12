using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using System;

public class OfficeEntranceSceneHandler : MonoBehaviour
{
    [SerializeField] TerminalHandler _terminalHandler;
    [SerializeField] CinemachineTriggerAction _cameraFoundTrigger;
    [SerializeField] GameObject _securityCameraRay;
    [SerializeField] PlayableDirector _secondChallengeCompleted;

    Vector3 _playerStartingPosition;

    private void Start() {
        GameStateHandler.Instance.OnGameStateChanged += OnGameStateChanged;
        _terminalHandler.OnChallengeCompleted += OnChallengeCompleted;

        _cameraFoundTrigger.enabled = !GameStateHandler.Instance.GameData.SecurityCameraFound;
        _securityCameraRay.SetActive(!GameStateHandler.Instance.GameData.SecondChallengeCompleted);
        _playerStartingPosition = PlayerController.Instance.transform.position;
    }

    private void OnDestroy() {
        GameStateHandler.Instance.OnGameStateChanged -= OnGameStateChanged;
        _terminalHandler.OnChallengeCompleted -= OnChallengeCompleted;
    }

    private void OnGameStateChanged(GameStateHandler.GameState gameState) {
        
    }

    public void ResetPlayerPosition() {
        PlayerController.Instance.transform.position = _playerStartingPosition;
    }

    private void OnChallengeCompleted() {
        if(GameStateHandler.Instance.GameData.SecondChallengeCompleted) return;
        _secondChallengeCompleted.Play();
        GameStateHandler.Instance.GameData.SecondChallengeCompleted = true;
    }

    public void SecurityCameraFound() {
        GameStateHandler.Instance.GameData.SecurityCameraFound = true;
    }
}
