using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rapresents the serialized GameData saved on the local storage.
/// </summary>

[System.Serializable]
public class GameSave
{
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

    public GameSave() { }

    public GameSave(GameData data)
    {
        CurrentScene = data.CurrentScene;
        PlayerPosition = data.PlayerPosition;
        PrevPlayerPosition = data.PrevPlayerPosition;
        HintTokenCount = data.HintTokenCount;
        IntroPlayed = data.IntroPlayed;
        FirstTokenFound = data.FirstTokenFound;
        TutorialCompleted = data.TutorialCompleted;
        FoundHintIDs = data.FoundHintIDs;
        UnlockedHintIDs = data.UnlockedHintIDs;
        BgMusicVolume = data.BgMusicVolume;
        SfxVolume = data.SfxVolume;
        InteractedWithFirstComputer = data.InteractedWithFirstComputer;
        InteractedWithMailApp = data.InteractedWithMailApp;
        ConnectedViaSsh = data.ConnectedViaSsh;
        NotesThought = data.NotesThought;
        FirstChallengeCompleted = data.FirstChallengeCompleted;
        EnteredOffice = data.EnteredOffice;
        InteractedWithSecondComputer = data.InteractedWithSecondComputer;
        SecurityCameraFound = data.SecurityCameraFound;
        SecondChallengeCompleted = data.SecondChallengeCompleted;
        InteractedWithKeypad = data.InteractedWithKeypad;
        OpenedServerDoor = data.OpenedServerDoor;
        InteractedWithThirdComputer = data.InteractedWithThirdComputer;
        ThirdChallengeCompleted = data.ThirdChallengeCompleted;
        ViewedImage = data.ViewedImage;
        ExtractedHiddenFolder = data.ExtractedHiddenFolder;
        FourthChallengeCompleted = data.FourthChallengeCompleted;
    }
}
