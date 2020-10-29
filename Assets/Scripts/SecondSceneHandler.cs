using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SecondSceneHandler : MonoBehaviour
{
    [SerializeField] TerminalHandler terminalHandler;
    [SerializeField] PlayableDirector secondChallengeCompleted;

    private void Start() {
        GameStateHandler.Instance.OnGameStateChanged += OnGameStateChanged;
        terminalHandler.OnChallengeCompleted += OnChallengeCompleted;
    }

    private void OnDestroy() {
        GameStateHandler.Instance.OnGameStateChanged -= OnGameStateChanged;
        terminalHandler.OnChallengeCompleted -= OnChallengeCompleted;
    }

    private void OnGameStateChanged(GameStateHandler.GameState gameState) {
        
    }

    private void OnChallengeCompleted() {
        secondChallengeCompleted.Play();
    }

    public void SecurityCameraFound() {
        GameStateHandler.Instance.GameData.SecurityCameraFound = true;
    }

    public void InteractedWithComputer() {
        GameStateHandler.Instance.GameData.InteractedWithComputer = true;
    }
}
