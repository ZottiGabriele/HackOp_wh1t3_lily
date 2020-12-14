using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeServerRoomHandler : MonoBehaviour
{
    [SerializeField] Cutscene _thirdChallengeCompletedCutscene;
    [SerializeField] Cutscene _imageViewedFirstTime;
    [SerializeField] GameObject _scrollViewCh3;
    [SerializeField] GameObject _scrollViewCh4;
    [SerializeField] GameObject _imgViewer;

    static GameObject imgViewer;
    static Cutscene imageViewedFirstTime;
    static Cutscene thirdChallengeCompletedCutscene;

    private void Start() {
        imgViewer = _imgViewer;
        imageViewedFirstTime = _imageViewedFirstTime;
        thirdChallengeCompletedCutscene = _thirdChallengeCompletedCutscene;
        _scrollViewCh3.SetActive(!GameStateHandler.Instance.GameData.ThirdChallengeCompleted);
        _scrollViewCh4.SetActive(GameStateHandler.Instance.GameData.ThirdChallengeCompleted);
    }

    public static void OnThirdChallengeCompleted() {
        thirdChallengeCompletedCutscene.Play();
        GameStateHandler.Instance.GameData.ThirdChallengeCompleted = true;
    }

    public static void ShowImg() {
        imgViewer.SetActive(true);
        imageViewedFirstTime.Play();
        GameStateHandler.Instance.GameData.ViewedImage = true;
    }
}
