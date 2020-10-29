using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/GameData", fileName = "New GameData")]
public class GameData : ScriptableObject
{
    public int HintTokenCount = 0;
    public bool FirstTokenFound = false;
    public bool SecurityCameraFound = false;
    public bool InteractedWithComputer = false;

    public bool FirstChallengeCompleted = false;
    public bool SecondChallengeCompleted = false;
    public bool ThirdChallengeCompleted = false;
    public bool FourthChallengeCompleted = false;

    public void Reset() {
        HintTokenCount = 0;
        FirstTokenFound = false;
        SecurityCameraFound = false;
        InteractedWithComputer = false;
    }
}
