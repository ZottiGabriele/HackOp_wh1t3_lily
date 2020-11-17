using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

//TODO: make this thing different per scene? State machine?
public class GameStateHandler : MonoBehaviour
{
    public static GameStateHandler Instance;
    
    public GameData GameData {get => _gameData;}
    public GameState CurrentGameState {get => _currentGameState;}

    public Action<GameState>  OnGameStateChanged = _ => {};
    public Action OnHintTokenUpdate = () => {};

    [SerializeField] GameData _gameData;
    [SerializeField] bool _saveGameDataDuringPlay = false;
    [SerializeField] GameState _currentGameState;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Debug.LogWarning("ATTENTION: " + this + " has been destroyed because of double singleton");
            Destroy(this);
        }
    }

    private void Start() {
        if(_gameData == null) {
            Debug.LogWarning("No GameData assigned to " + this + " -> creating new instance");
            _gameData = ScriptableObject.CreateInstance<GameData>();
            return;
        } else if(!_saveGameDataDuringPlay) {
            _gameData = Instantiate(_gameData);
        }

        #if UNITY_EDITOR
        Application.targetFrameRate = 144;
        #endif
    }

    public void AddHintToken(int n) {
        if(!_gameData.FirstTokenFound) {
            GeneralUIHandler.Instance.ShowPopUp(GeneralUIHandler.PopUpType.FirstTokenFound);
            _gameData.FirstTokenFound = true;
        }
        _gameData.HintTokenCount += n;
        OnHintTokenUpdate();
    }

    public void ChengeScene(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }

    private void ChangeGameState(GameState newGameState) {
        _currentGameState = newGameState;
        OnGameStateChanged(_currentGameState);
    }

    public void GameOver() {
        ChangeGameState(GameState.Gameover);
    }

    public void UnpausableCutsceneStarted() {
        ChangeGameState(GameState.UnpausableCutscene);
    }

    public void UnpausableCutsceneEnded() {
        ChangeGameState(GameState.Playing);
    }

    public void PauseGame() {
        if(_currentGameState == GameState.UnpausableCutscene || _currentGameState == GameState.Gameover) return;
        Time.timeScale = 0;
        PlayerController.Instance.DisableInput();
        ChangeGameState(GameState.Paused);
    }

    public void ResumeGame() {
        Time.timeScale = 1;
        PlayerController.Instance.RestoreInput();
        ChangeGameState(GameState.Playing);
    }

    public void ExitGame() {
        Application.Quit();
    }

    public enum GameState {
        Playing,
        Paused,
        Cutscene,
        UnpausableCutscene,
        Gameover
    }
}
