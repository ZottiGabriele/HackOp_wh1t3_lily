using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingRoomSceneHandler : MonoBehaviour
{
    [SerializeField] GameObject _mailNotification;
    [SerializeField] Cutscene _sshCutscene;
    [SerializeField] Cutscene _firstChallengeCompleted;

    private void Start()
    {
        _mailNotification.SetActive(!GameStateHandler.Instance.GameData.InteractedWithMailApp);
        SshCommand.OnConnectionSuccessfull += onConnectionSuccessfull;
        GenerateDailyCodeCommand.OnDailyCodeGenerated += onDailyCodeGenerated;
    }

    private void OnDestroy()
    {
        SshCommand.OnConnectionSuccessfull -= onConnectionSuccessfull;
        GenerateDailyCodeCommand.OnDailyCodeGenerated -= onDailyCodeGenerated;
    }

    private void onConnectionSuccessfull()
    {
        TerminalHandler.Instance.DisplayOutput("HackOp offices welcome message:\n" +
                                               "Welcome back wh1t3_lily. The office will open again at 9:00 am tomorrow and you will receive the new daily access code.\n" +
                                               "Remember to use the terminal wisely!");
        _sshCutscene.Play();
        GameStateHandler.Instance.GameData.ConnectedViaSsh = true;
        GameStateHandler.Instance.SaveGame();
    }

    private void onDailyCodeGenerated()
    {
        _firstChallengeCompleted.Play();
        GameStateHandler.Instance.GameData.FirstChallengeCompleted = true;
        GameStateHandler.Instance.SaveGame();
    }

    public void InteractedWithMailApp()
    {
        GameStateHandler.Instance.GameData.InteractedWithMailApp = true;
        GameStateHandler.Instance.SaveGame();
        _mailNotification.SetActive(false);
    }
}
