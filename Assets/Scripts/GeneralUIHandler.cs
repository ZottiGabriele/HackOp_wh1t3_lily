using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GeneralUIHandler : MonoBehaviour
{
    public static GeneralUIHandler Instance { get; private set; }

    [SerializeField] GameObject _textBox;
    [SerializeField] GameObject _firstTokenFoundPopUp;
    [SerializeField] GameObject _menu;
    [SerializeField] GameObject _settings;
    [SerializeField] UnityEngine.UI.Slider _bgMusicSlider;
    [SerializeField] UnityEngine.UI.Slider _sfxSlider;
    [Range(0, 1)] public float _typingSpeed = 0.95f;
    [SerializeField] TMP_Text _text;

    [Header("Scene Specific Data")]
    [Space(25)]

    [SerializeField] Cutscene fadeOut;
    [SerializeField] Cutscene fadein;

    private bool isActive = true;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            // DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Debug.LogWarning("ATTENTION: " + this + " has been destroyed because of double singleton");
            Destroy(this);
        }
    }

    private void Start()
    {
        _bgMusicSlider.value = GameStateHandler.Instance.GameData.BgMusicVolume;
        _sfxSlider.value = GameStateHandler.Instance.GameData.SfxVolume;
    }

    public void ShowText(string text)
    {
        _textBox.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(startTypingEffect(text));
    }

    //USED BY TIMELINE INTEGRATION
    public void PlayText(string text)
    {
        _textBox.SetActive(true);
        if (Application.isPlaying && text != _text.text) SoundsHandler.Instance.PlayTextSound();
        _text.text = text;
    }

    //USED BY TIMELINE INTEGRATION
    public void StopText()
    {
        _textBox.SetActive(false);
    }

    public void ShowPopUp(PopUpType popUpType)
    {
        switch (popUpType)
        {
            case PopUpType.FirstTokenFound:
                StartCoroutine(showFirstTokenFoundPopUp());
                break;
        }
    }

    public void ShowMenu()
    {
        switch (GameStateHandler.Instance.CurrentGameState)
        {
            case GameStateHandler.GameState.UnpausableCutscene:
            case GameStateHandler.GameState.InteractingWithComputer:
                return;
            default:
                _menu.SetActive(true);
                GameStateHandler.Instance.PauseGame();
                break;
        }
    }

    public void HideMenu()
    {
        _menu.SetActive(false);
        GameStateHandler.Instance.ResumeGame();
    }

    public void ToggleMenu()
    {
        if (_settings.activeInHierarchy)
        {
            HideSettings();
        }

        if (_menu.activeInHierarchy)
        {
            HideMenu();
        }
        else
        {
            ShowMenu();
        }
    }

    public void OnSaveGamePressed()
    {
        GameStateHandler.Instance.SaveGame();
    }

    public void OnLoadGamePressed()
    {
        HideMenu();
        fadeOut.ForcePlay(() => StartCoroutine(loadGameTransition()));
    }

    public void ShowSettings()
    {
        HideMenu();
        GameStateHandler.Instance.PauseGame();
        _settings.SetActive(true);
    }

    public void HideSettings()
    {
        _settings.SetActive(false);
        ShowMenu();
    }

    public void OnExitGamePressed()
    {
        GameStateHandler.Instance.ExitGame();
    }

    public void OnMusicSliderUpdate(float value)
    {
        SoundsHandler.Instance.BgMusicVolume = value;
    }

    public void OnSfxSliderUpdate(float value)
    {
        SoundsHandler.Instance.SfxVolume = value;
    }

    IEnumerator loadGameTransition()
    {
        PlayerController.ShouldLoadPosition = true;
        GameStateHandler.Instance.ReloadLastSave();
        yield return new WaitForSeconds(0.5f);
        fadein.ForcePlay();
    }

    IEnumerator showFirstTokenFoundPopUp()
    {
        PlayerController.Instance.DisableInput();
        yield return new WaitForSeconds(2.5f);
        _firstTokenFoundPopUp.SetActive(true);
    }

    IEnumerator startTypingEffect(string text)
    {
        _text.text = "";
        foreach (var c in text)
        {
            SoundsHandler.Instance.PlayTextSound();
            _text.text += c;
            yield return new WaitForSeconds(1 - _typingSpeed);
        }
        yield return new WaitForSeconds(2);
        _textBox.SetActive(false);
    }

    public enum PopUpType
    {
        FirstTokenFound
    }
}
