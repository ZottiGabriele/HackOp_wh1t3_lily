using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class StartingRoomSceneHandler : MonoBehaviour
{
    [SerializeField] GameObject _mailNotification;
    [SerializeField] Cutscene _introCutscene;
    [SerializeField] Cutscene _sshCutscene;
    [SerializeField] Cutscene _firstChallengeCompleted;
    [SerializeField] Cutscene _notesThought;
    [SerializeField] TerminalHandler _terminalHandler;

    private void Start()
    {
        _mailNotification.SetActive(!GameStateHandler.Instance.GameData.InteractedWithMailApp);
        SshCommand.OnConnectionSuccessfull += onConnectionSuccessfull;
        GenerateDailyCodeCommand.OnDailyCodeGenerated += onDailyCodeGenerated;
        _terminalHandler.OnInputProcessed += onInputProcessed;
    }


    private void OnDestroy()
    {
        SshCommand.OnConnectionSuccessfull -= onConnectionSuccessfull;
        GenerateDailyCodeCommand.OnDailyCodeGenerated -= onDailyCodeGenerated;
        _terminalHandler.OnInputProcessed -= onInputProcessed;
    }

    private void onInputProcessed(string input)
    {
        if (GameStateHandler.Instance.GameData.NotesThought) return;

        Match m = Regex.Match(input, "^ *cat +[\\S]+ *$");
        if (m.Success)
        {
            var parts = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var target = TerminalHandler.Instance.VirtualFileSystem.Query(parts[1]);
            if (target != null && target.full_path == "/home/user/notes.txt")
            {
                _notesThought.Play();
                GameStateHandler.Instance.GameData.NotesThought = true;
                GameStateHandler.Instance.SaveGame();
            }
        }
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

    public void InteractingWithComputer(bool isInteracting)
    {
        GameStateHandler.Instance.InteractingWithComputer(isInteracting);
    }

    public void UnpausableCutsceneStarted()
    {
        GameStateHandler.Instance.UnpausableCutsceneStarted();
    }

    public void UnpausableCutsceneEnded()
    {
        GameStateHandler.Instance.UnpausableCutsceneEnded();
    }
}
