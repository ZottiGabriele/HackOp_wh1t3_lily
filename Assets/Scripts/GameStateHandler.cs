using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

//TODO: make this thing different per scene? State machine?
public class GameStateHandler : MonoBehaviour
{
    public static GameStateHandler Instance;
    
    public GameData GameData {get => _gameData;}

    [SerializeField] GameData _gameData;
    [SerializeField] bool _saveGameDataDuringPlay = false;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
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
        _gameData.HintTokenCount += n;
    }

//TODO: here? Should it work this way?
    public void SecurityCameraFound() {
        _gameData.SecurityCameraFound = true;
    }

    public void InteractedWithComputer() {
        _gameData.InteractedWithComputer = true;
    }
}
