using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingRoomSceneHandler : MonoBehaviour
{
    [SerializeField] GameObject _mailNotification;

    private void Start() {
        _mailNotification.SetActive(!GameStateHandler.Instance.GameData.InteractedWithMailApp);
    }

    public void InteractedWithMailApp() {
        GameStateHandler.Instance.GameData.InteractedWithMailApp = true;
        _mailNotification.SetActive(false);
    }
}
