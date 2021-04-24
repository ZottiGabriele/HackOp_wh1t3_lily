using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! IF ANYTHING IS RENAMED CHECK CONDITIONS ON IINTERACTIONAREA

/// <summary>
/// ScriptableObject that rapreents the instance of the game data.
/// </summary>
[System.Serializable]
[CreateAssetMenu(menuName = "Custom/GameData", fileName = "New GameData")]
public class GameData : ScriptableObject
{
    //GENERAL
    public int CurrentScene = 1;
    public float[] PlayerPosition = { 4.7f, 0, 0 };
    public float[] PrevPlayerPosition = { 0, 0, 0 };
    public int HintTokenCount = 0;
    public bool IntroPlayed = false;
    public bool FirstTokenFound = false;
    public bool TutorialCompleted = false;
    public List<string> FoundHintIDs = new List<string>();
    public List<string> UnlockedHintIDs = new List<string>();

    //SETTINGS
    public float BgMusicVolume = 1;
    public float SfxVolume = 1;

    //FIRST CHALLENGE
    public bool InteractedWithFirstComputer = false;
    public bool InteractedWithMailApp = false;
    public bool ConnectedViaSsh = false;
    public bool NotesThought = false;
    public bool FirstChallengeCompleted = false;

    //SECOND CHALLENGE
    public bool EnteredOffice = false;
    public bool InteractedWithSecondComputer = false;
    public bool SecurityCameraFound = false;
    public bool SecondChallengeCompleted = false;

    //THIRD CHALLENGE
    public bool InteractedWithKeypad = false;
    public bool OpenedServerDoor = false;
    public bool InteractedWithThirdComputer = false;
    public bool ThirdChallengeCompleted = false;

    //FOURTH CHALLENGE
    public bool ViewedImage = false;
    public bool ExtractedHiddenFolder = false;
    public bool FourthChallengeCompleted = false;

    public void LoadGameSave(GameSave save)
    {
        CurrentScene = save.CurrentScene;
        PlayerPosition = save.PlayerPosition;
        PrevPlayerPosition = save.PrevPlayerPosition;
        HintTokenCount = save.HintTokenCount;
        IntroPlayed = save.IntroPlayed;
        FirstTokenFound = save.FirstTokenFound;
        TutorialCompleted = save.TutorialCompleted;
        FoundHintIDs = save.FoundHintIDs;
        UnlockedHintIDs = save.UnlockedHintIDs;
        BgMusicVolume = save.BgMusicVolume;
        SfxVolume = save.SfxVolume;
        InteractedWithFirstComputer = save.InteractedWithFirstComputer;
        InteractedWithMailApp = save.InteractedWithMailApp;
        ConnectedViaSsh = save.ConnectedViaSsh;
        NotesThought = save.NotesThought;
        FirstChallengeCompleted = save.FirstChallengeCompleted;
        EnteredOffice = save.EnteredOffice;
        InteractedWithSecondComputer = save.InteractedWithSecondComputer;
        SecurityCameraFound = save.SecurityCameraFound;
        SecondChallengeCompleted = save.SecondChallengeCompleted;
        InteractedWithKeypad = save.InteractedWithKeypad;
        OpenedServerDoor = save.OpenedServerDoor;
        InteractedWithThirdComputer = save.InteractedWithThirdComputer;
        ThirdChallengeCompleted = save.ThirdChallengeCompleted;
        ViewedImage = save.ViewedImage;
        ExtractedHiddenFolder = save.ExtractedHiddenFolder;
        FourthChallengeCompleted = save.FourthChallengeCompleted;
    }
}
