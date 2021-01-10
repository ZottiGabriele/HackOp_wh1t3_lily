using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{

    [SerializeField] GameObject _continue;
    [SerializeField] Cutscene _outro;
    [SerializeField] Cutscene _intro_pt1;

    RoamingUiHandler _rui;

    private void Start()
    {
        var gameDataFound = GameStateHandler.Instance.LoadGame();

        _continue.SetActive(gameDataFound);

        PlayerController.ShouldLoadPosition = true;

        SceneManager.LoadSceneAsync(GameStateHandler.Instance.GameData.CurrentScene, LoadSceneMode.Additive).completed +=
            _ => StartCoroutine(WaitForSceneInit());
    }

    private IEnumerator WaitForSceneInit()
    {
        yield return new WaitForEndOfFrame();
        PlayerController.Instance.DisableInput();
        _rui = FindObjectOfType<RoamingUiHandler>();
        if (_rui != null) _rui.gameObject.SetActive(false);
    }

    public void OnNewGame()
    {
        int currentScene = GameStateHandler.Instance.GameData.CurrentScene;
        _outro.ForcePlay(() =>
        {
            SceneManager.UnloadSceneAsync(currentScene);
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
        });
        _intro_pt1.ForcePlay(closeMainMenu);
        GameStateHandler.Instance.NewGame();
    }

    public void OnContinue()
    {
        _outro.ForcePlay(() =>
        {
            closeMainMenu();
            PlayerController.Instance.EnableInput();
            if (_rui != null) _rui.gameObject.SetActive(true);
        });
    }

    private void closeMainMenu()
    {
        var intro_pt2_obj = GameObject.FindGameObjectWithTag("Intro_pt2");
        if (intro_pt2_obj != null)
        {
            var intro_pt2 = intro_pt2_obj.GetComponent<Cutscene>();
            if (!GameStateHandler.Instance.GameData.IntroPlayed) intro_pt2.ForcePlay(() =>
                {
                    GameStateHandler.Instance.GameData.IntroPlayed = true;
                });
        }

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(0));
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
}
