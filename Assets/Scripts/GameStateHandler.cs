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
    public string SAVE_PATH { get => Application.persistentDataPath + "/gamedata.save"; }

    public static GameStateHandler Instance;

    public GameData GameData { get => _gameData; }
    public GameState CurrentGameState { get => _gameStateHistory.Peek(); }

    public Action<GameState> OnGameStateChanged = _ => { };
    public Action OnHintTokenUpdate = () => { };

    [SerializeField] GameData _gameData;
    [SerializeField] bool _saveGameDataDuringPlay = false;
    [SerializeField] bool _loadDataFromSaveFile = true;
    [SerializeField] GameState _startingGameState = GameState.Playing;
    [SerializeField] Texture2D _pointer;
    private Stack<GameState> _gameStateHistory = new Stack<GameState>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            InitializeGame();
        }
        else
        {
            Debug.LogWarning("ATTENTION: " + this + " has been destroyed because of double singleton");
            Destroy(this.gameObject);
        }
    }

    private void InitializeGame()
    {
        _gameStateHistory = new Stack<GameState>();
        _gameStateHistory.Push(_startingGameState);

        //if custom game data manually assigned from editor, just use that
        if (_gameData != null)
        {
            _gameData = Instantiate(_gameData);
            Debug.LogWarning("Custom GameData assigned: GAME WILL NOT LOAD SAVE DATA!");
        }
        else
        {
            _gameData = ScriptableObject.CreateInstance<GameData>();

            if (_loadDataFromSaveFile)
            {
                LoadGame();
            }
        }


        // SceneManager.sceneLoaded += (_, __) => SaveGame();
        QualitySettings.vSyncCount = 1;
        Cursor.SetCursor(_pointer, Vector2.one * 6, CursorMode.Auto);

#if UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif
    }

    public void NewGame()
    {
        if (File.Exists(SAVE_PATH))
        {
            File.Delete(SAVE_PATH);
            _gameData = null;
            InitializeGame();
        }
    }

    public void SaveGame()
    {
        var binaryFormatter = new BinaryFormatter();
        _gameData.CurrentScene = SceneManager.GetActiveScene().buildIndex;
        _gameData.PlayerPosition = new float[] {
            PlayerController.Instance.gameObject.transform.position.x,
            PlayerController.Instance.gameObject.transform.position.y,
            PlayerController.Instance.gameObject.transform.position.z
        };
        var save = new GameSave(_gameData);
        using (var fileStream = File.Create(SAVE_PATH))
        {
            binaryFormatter.Serialize(fileStream, save);
        }
    }

    public bool LoadGame()
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
            return true;
        }

        return false;
    }

    public void ReloadLastSave()
    {
        LoadGame();
        ChangeScene(_gameData.CurrentScene);
    }

    public void AddHintToken(string GUID)
    {
        if (!_gameData.FirstTokenFound)
        {
            GeneralUIHandler.Instance.ShowPopUp(GeneralUIHandler.PopUpType.FirstTokenFound);
            _gameData.FirstTokenFound = true;
        }
        _gameData.HintTokenCount++;
        _gameData.FoundHintIDs.Add(GUID);
        OnHintTokenUpdate();
        SaveGame();
    }

    public void UseHintToken()
    {
        _gameData.HintTokenCount--;
        OnHintTokenUpdate();
        SaveGame();
    }

    public void ChangeScene(int sceneIndex)
    {
        bool goingBack = sceneIndex < _gameData.CurrentScene;

        SceneManager.LoadSceneAsync(sceneIndex).completed += _ =>
        {
            if (goingBack) PlayerController.Instance.transform.position = ISceneHandler.Instance.GoBackPosition;
            SaveGame();
        };
    }

    private void ChangeGameState(GameState newGameState)
    {
        var prev = CurrentGameState;
        _gameStateHistory.Push(newGameState);
        Cursor.visible = !(CurrentGameState == GameState.UnpausableCutscene);
        OnGameStateChanged(CurrentGameState);
        Debug.Log(prev + " --> " + CurrentGameState);
    }

    private void ResumePrevGameState()
    {
        var prev = CurrentGameState;
        _gameStateHistory.Pop();
        Cursor.visible = !(CurrentGameState == GameState.UnpausableCutscene);
        OnGameStateChanged(CurrentGameState);
        Debug.Log(prev + " --> " + CurrentGameState);
    }

    public void GameOver()
    {
        ReloadLastSave();
    }

    public void UnpausableCutsceneStarted()
    {
        ChangeGameState(GameState.UnpausableCutscene);
    }

    public void UnpausableCutsceneEnded()
    {
        ResumePrevGameState();
    }

    public void InteractingWithComputer(bool isInteracting)
    {
        if (isInteracting)
        {
            ChangeGameState(GameState.InteractingWithComputer);
        }
        else
        {
            ResumePrevGameState();
        }
    }

    public void PauseGame()
    {
        if (CurrentGameState == GameState.UnpausableCutscene) return;
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

#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
    }

    public void CompleteGame()
    {
        if (File.Exists(SAVE_PATH))
        {
            File.Delete(SAVE_PATH);
        }
        Application.Quit();
    }

    public enum GameState
    {
        Playing,
        Paused,
        UnpausableCutscene,
        InteractingWithComputer
    }
}
