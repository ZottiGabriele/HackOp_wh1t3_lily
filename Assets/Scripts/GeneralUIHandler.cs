using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GeneralUIHandler : MonoBehaviour
{
    public static GeneralUIHandler Instance {get; private set;}

    [SerializeField] GameObject _textBox;
    [SerializeField] GameObject _firstTokenFoundPopUp;
    [SerializeField] GameObject _menu;
    [SerializeField] [Range(0,1)] float typingSpeed = 0.95f;
    [SerializeField] TMP_Text _text;

    private bool isActive = true;

    private void Awake()
    {
        if(!Instance) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else if (Instance != this) {
            Destroy(this);
        }
    }

    private void Update() {
        if(GameStateHandler.Instance.CurrentGameState == GameStateHandler.GameState.Gameover ||
           GameStateHandler.Instance.CurrentGameState == GameStateHandler.GameState.UnpausableCutscene) return;
        
        if(Input.GetKeyDown(KeyCode.Escape)) {
            ToggleMenu();
        }
    }

    public void ShowText(string text) {
        _textBox.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(startTypingEffect(text));
    }

    public void ShowPopUp(PopUpType popUpType) {
        switch(popUpType) {
            case PopUpType.FirstTokenFound:
                StartCoroutine(showFirstTokenFoundPopUp());
                break;
        }
    }

    public void ShowMenu() {
        if(GameStateHandler.Instance.CurrentGameState == GameStateHandler.GameState.Gameover ||
           GameStateHandler.Instance.CurrentGameState == GameStateHandler.GameState.UnpausableCutscene) return;
        _menu.SetActive(true);
        GameStateHandler.Instance.PauseGame();
    }

    public void HideMenu() {
        _menu.SetActive(false);
        GameStateHandler.Instance.ResumeGame();
    }

    public void ToggleMenu() {
        if(_menu.activeInHierarchy) {
            HideMenu();
        } else {
            ShowMenu();
        }
    }

    IEnumerator showFirstTokenFoundPopUp() {
        PlayerController.Instance.DisableInput();
        yield return new WaitForSeconds(1.8f);
        _firstTokenFoundPopUp.SetActive(true);
    }

    IEnumerator startTypingEffect(string text) {
        _text.text = "";
        foreach(var c in text) {
            _text.text += c;
            yield return new WaitForSeconds(1 - typingSpeed);
        }
        yield return new WaitForSeconds(2);
        _textBox.SetActive(false);
    }

    public enum PopUpType {
        FirstTokenFound
    }
}
