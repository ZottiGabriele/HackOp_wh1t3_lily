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
}
