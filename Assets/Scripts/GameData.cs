using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! IF ANYTHING IS RENAMED CHECK CONDITIONS ON IINTERACTIONAREA
[System.Serializable]
[CreateAssetMenu(menuName = "Custom/GameData", fileName = "New GameData")]
public class GameData : ScriptableObject
{
    //GENERAL
    public int HintTokenCount = 0;
    public bool FirstTokenFound = false;

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
    public bool InteractedWithThirdComputer = false;
    public bool ThirdChallengeCompleted = false;

    //FOURTH CHALLENGE
    public bool InteractedWithFourthComputer = false;
    public bool FourthChallengeCompleted = false;


    public void Reset() {
        HintTokenCount = 0;
        FirstTokenFound = false;
        SecurityCameraFound = false;
        
        InteractedWithFirstComputer = false;
        InteractedWithSecondComputer = false;
        InteractedWithThirdComputer = false;
        InteractedWithFourthComputer = false;
        
        FirstChallengeCompleted = false;
        SecondChallengeCompleted = false;
        ThirdChallengeCompleted = false;
        FourthChallengeCompleted = false;
    }
}
