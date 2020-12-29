using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSave
{
    public int CurrentScene = 0;
    public int HintTokenCount = 0;
    public bool FirstTokenFound = false;
    public List<int> FoundHintIDs = new List<int>();
    public List<int> BoughtHintIDs = new List<int>();

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

    public GameSave() { }

    public GameSave(GameData data)
    {
        CurrentScene = data.CurrentScene;
        HintTokenCount = data.HintTokenCount;
        FirstTokenFound = data.FirstTokenFound;
        FoundHintIDs = data.FoundHintIDs;
        BoughtHintIDs = data.BoughtHintIDs;
        InteractedWithFirstComputer = data.InteractedWithFirstComputer;
        InteractedWithMailApp = data.InteractedWithMailApp;
        ConnectedViaSsh = data.ConnectedViaSsh;
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
