using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameStateHandler : MonoBehaviour
{
    private string SAVE_PATH { get => Application.persistentDataPath + "/gamedata.save"; }

    public static GameStateHandler Instance;

    public GameData GameData { get => _gameData; }
    public GameState CurrentGameState { get => _currentGameState; }

    public Action<GameState> OnGameStateChanged = _ => { };
    public Action OnHintTokenUpdate = () => { };

    [SerializeField] GameData _gameData;
    [SerializeField] bool _saveGameDataDuringPlay = false;
    [SerializeField] bool _loadDataFromSaveFile = true;
    [SerializeField] GameState _currentGameState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.LogWarning("ATTENTION: " + this + " has been destroyed because of double singleton");
            Destroy(this);
        }
    }

    private void Start()
    {
        if (_loadDataFromSaveFile)
        {
            LoadGame();
        }

        if (_gameData == null)
        {
            Debug.LogWarning("No GameData assigned to " + this + " -> creating new instance");
            _gameData = ScriptableObject.CreateInstance<GameData>();
        }

        if (!_saveGameDataDuringPlay)
        {
            _gameData = Instantiate(_gameData);
        }

        SceneManager.sceneLoaded += (_, __) => SaveGame();
        QualitySettings.vSyncCount = 1;

#if UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif
    }

    public void SaveGame()
    {
        var binaryFormatter = new BinaryFormatter();
        _gameData.CurrentScene = SceneManager.GetActiveScene().buildIndex;
        var save = new GameSave(_gameData);
        using (var fileStream = File.Create(SAVE_PATH))
        {
            binaryFormatter.Serialize(fileStream, save);
        }
    }

    public void LoadGame()
    {
        if (File.Exists(SAVE_PATH))
        {
            var binaryFormatter = new BinaryFormatter();
            var save = new GameSave();
            using (var fileStream = File.Open(SAVE_PATH, FileMode.Open))
            {
                save = binaryFormatter.Deserialize(fileStream) as GameSave;
            }
            if (_gameData == null) _gameData = ScriptableObject.CreateInstance<GameData>();
            _gameData.LoadGameSave(save);
            if (SceneManager.GetActiveScene().buildIndex != _gameData.CurrentScene) ChangeScene(_gameData.CurrentScene);
        }
    }

    public void AddHintToken(int ID)
    {
        if (!_gameData.FirstTokenFound)
        {
            GeneralUIHandler.Instance.ShowPopUp(GeneralUIHandler.PopUpType.FirstTokenFound);
            _gameData.FirstTokenFound = true;
        }
        _gameData.HintTokenCount++;
        _gameData.FoundHintIDs.Add(ID);
        OnHintTokenUpdate();
    }

    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    private void ChangeGameState(GameState newGameState)
    {
        _currentGameState = newGameState;
        OnGameStateChanged(_currentGameState);
    }

    public void GameOver()
    {
        ChangeGameState(GameState.Gameover);
    }

    public void UnpausableCutsceneStarted()
    {
        ChangeGameState(GameState.UnpausableCutscene);
    }

    public void UnpausableCutsceneEnded()
    {
        ChangeGameState(GameState.Playing);
    }

    public void PauseGame()
    {
        if (_currentGameState == GameState.UnpausableCutscene || _currentGameState == GameState.Gameover) return;
        Time.timeScale = 0;
        PlayerController.Instance.DisableInput();
        ChangeGameState(GameState.Paused);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        PlayerController.Instance.RestoreInput();
        ChangeGameState(GameState.Playing);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public enum GameState
    {
        Playing,
        Paused,
        Cutscene,
        UnpausableCutscene,
        Gameover
    }
}
