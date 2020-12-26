using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! IF ANYTHING IS RENAMED CHECK CONDITIONS ON IINTERACTIONAREA
[System.Serializable]
[CreateAssetMenu(menuName = "Custom/GameData", fileName = "New GameData")]
public class GameData : ScriptableObject
{
    //GENERAL
    public int CurrentScene = 0;
    public int HintTokenCount = 0;
    public bool FirstTokenFound = false;
    public List<int> FoundHintIDs = new List<int>();

    //FIRST CHALLENGE
    public bool InteractedWithFirstComputer = false;
    public bool InteractedWithMailApp = false;
    public bool ConnectedViaSsh = false;
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
        HintTokenCount = save.HintTokenCount;
        FirstTokenFound = save.FirstTokenFound;
        FoundHintIDs = save.FoundHintIDs;
        InteractedWithFirstComputer = save.InteractedWithFirstComputer;
        InteractedWithMailApp = save.InteractedWithMailApp;
        ConnectedViaSsh = save.ConnectedViaSsh;
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
