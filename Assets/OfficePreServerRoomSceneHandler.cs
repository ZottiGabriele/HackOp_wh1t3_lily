using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficePreServerRoomSceneHandler : ISceneHandler
{
    public void RightCodeEntered()
    {
        GameStateHandler.Instance.GameData.OpenedServerDoor = true;
        GameStateHandler.Instance.SaveGame();
    }

    public void InteractingWithKeypad(bool isInteracting)
    {
        GameStateHandler.Instance.InteractingWithComputer(isInteracting);
    }
}
