using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficePreServerRoomSceneHandler : MonoBehaviour
{
    public void RightCodeEntered()
    {
        GameStateHandler.Instance.GameData.OpenedServerDoor = true;
        GameStateHandler.Instance.SaveGame();
    }

    public void GameOver()
    {
        GameStateHandler.Instance.GameOver();
    }

    public void OnUnpausableCutsceneStarted()
    {
        GameStateHandler.Instance.UnpausableCutsceneStarted();
    }

    public void OnUnpausableCutsceneEnded()
    {
        GameStateHandler.Instance.UnpausableCutsceneEnded();
    }

    public void InteractingWithKeypad(bool isInteracting)
    {
        GameStateHandler.Instance.InteractingWithComputer(isInteracting);
    }
}
