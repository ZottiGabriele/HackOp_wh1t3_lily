using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

[RequireComponent(typeof(PolygonCollider2D))]
public class ComputerInteractionArea : IInteractionArea
{
    [SerializeField] Cutscene _firstInteraction;
    [SerializeField] Cutscene _zoomInOnScreen;
    [SerializeField] TerminalConfig.Challenge _targetChallenge;

    bool _interacted = false;

    protected override void execute()
    {

        switch (_targetChallenge)
        {
            case TerminalConfig.Challenge.zero:
                _interacted = GameStateHandler.Instance.GameData.InteractedWithFirstComputer;
                GameStateHandler.Instance.GameData.InteractedWithFirstComputer = true;
                break;
            case TerminalConfig.Challenge.second:
                _interacted = GameStateHandler.Instance.GameData.InteractedWithSecondComputer;
                GameStateHandler.Instance.GameData.InteractedWithSecondComputer = true;
                break;
            case TerminalConfig.Challenge.third:
                _interacted = GameStateHandler.Instance.GameData.InteractedWithThirdComputer;
                GameStateHandler.Instance.GameData.InteractedWithThirdComputer = true;
                break;
            case TerminalConfig.Challenge.fourth:
                //TODO: remove ?
                // _interacted = GameStateHandler.Instance.GameData.InteractedWithFourthComputer;
                // GameStateHandler.Instance.GameData.InteractedWithFourthComputer = true;
                break;
        }

        if (!_interacted)
        {
            _firstInteraction.Play();
            GameStateHandler.Instance.SaveGame();
        }
        else
        {
            _zoomInOnScreen.Play();
        }
    }
}
