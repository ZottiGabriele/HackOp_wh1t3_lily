using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OfficeServerRoomHandler : ISceneHandler
{
    [SerializeField] Cutscene _thirdChallengeCompletedCutscene;
    [SerializeField] Cutscene _fourthChallengeCompletedCutscene;
    [SerializeField] Cutscene _imageViewedFirstTime;
    [SerializeField] GameObject _imgViewer;
    [SerializeField] GameObject _partOneSideBar;
    [SerializeField] GameObject _partTwoSideBar;

    static GameObject imgViewer;
    static GameObject partOneSideBar;
    static GameObject partTwoSideBar;
    static Cutscene imageViewedFirstTime;
    static Cutscene thirdChallengeCompletedCutscene;
    static Cutscene fourthChallengeCompletedCutscene;

    private void Start()
    {
        imgViewer = _imgViewer;
        partOneSideBar = _partOneSideBar;
        partTwoSideBar = _partTwoSideBar;
        imageViewedFirstTime = _imageViewedFirstTime;
        thirdChallengeCompletedCutscene = _thirdChallengeCompletedCutscene;
        fourthChallengeCompletedCutscene = _fourthChallengeCompletedCutscene;

        partOneSideBar.SetActive(!GameStateHandler.Instance.GameData.ThirdChallengeCompleted);
        partTwoSideBar.SetActive(GameStateHandler.Instance.GameData.ThirdChallengeCompleted);

        GameStateHandler.Instance.SaveGame();
    }

    public static void OnThirdChallengeCompleted()
    {
        thirdChallengeCompletedCutscene.Play();
        GameStateHandler.Instance.GameData.ThirdChallengeCompleted = true;
        partOneSideBar.SetActive(false);
        partTwoSideBar.SetActive(true);
        GameStateHandler.Instance.SaveGame();
    }

    public static void ExtractHiddenFolder()
    {
        var gibberish = new VirtualFileSystemEntry(false, true, "gibberish.txt", "/home/.wh1t3_l1ly/nothing_to_see_here/gibberish.txt", "/home/_ro_h_wh1t3_l1ly/nothing_to_see_here/gibberish.txt", "-rw-------", "root", "root", "file",
            "Why even look at this gibberish?\n" +
            "\n" +
            "54 6b 46 47 53 6c 4a 46 65 31 4e 43 52 55 64 4d 52 30 70 43 66 51 3d 3d\n" +
            "\n" +
            "You do you...",
            new VirtualFileSystemEntry[0]
        );

        var folder = new VirtualFileSystemEntry(false, true, "nothing_to_see_here", "/home/.wh1t3_l1ly/nothing_to_see_here", "/home/_ro_h_wh1t3_l1ly/nothing_to_see_here", "drwx------", "root", "root", "directory",
            "",
            new VirtualFileSystemEntry[] { gibberish }
        );

        TerminalHandler.Instance.VirtualFileSystem.AddEntry(folder);
        TerminalHandler.Instance.VirtualFileSystem.AddEntry(gibberish);
        TerminalHandler.Instance.DisplayOutput("Archive:  wh1t3_l1ly.jpg\n" +
            "warning[wh1t3_l1ly.jpg]:  27295 extra bytes at beginning or within zipfile\n" +
            "  ls -a(attempting to process anyway)\n" +
            "  inflating: nothing_to_see_here / gibberish.txt");
        GameStateHandler.Instance.GameData.ExtractedHiddenFolder = true;
        GameStateHandler.Instance.SaveGame();
    }


    public static void OnFourthChallengeCompelted()
    {
        fourthChallengeCompletedCutscene.Play();
        GameStateHandler.Instance.GameData.FourthChallengeCompleted = true;
        GameStateHandler.Instance.SaveGame();
    }

    public static void ShowImg()
    {
        imgViewer.SetActive(true);
        imageViewedFirstTime.Play();
        GameStateHandler.Instance.GameData.ViewedImage = true;
    }

    public void CreditsEnded()
    {
        GameStateHandler.Instance.CompleteGame();
    }
}
