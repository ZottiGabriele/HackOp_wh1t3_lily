using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

//TODO: is this ok?
//TODO: review with new condition settings, maybe remove completly
public class SecondChallengeInteractionArea : IInteractionArea
{
    [SerializeField] string _preChallangeText;
    [SerializeField] string _onChallangeText;
    [SerializeField] PlayableDirector _zoomInOnScreen;
    [SerializeField] float _targetTimeIfAlreadyInteracted = 3.6f;

    protected override void onInteraction()
    {
        if(!GameStateHandler.Instance.GameData.SecurityCameraFound) {
            GeneralUIHandler.Instance.ShowText(_preChallangeText);
        } else {
            if(!GameStateHandler.Instance.GameData.InteractedWithComputer) {
                GeneralUIHandler.Instance.ShowText(_onChallangeText);
                _zoomInOnScreen.Play();
            } else {
                _zoomInOnScreen.time = _targetTimeIfAlreadyInteracted;
                _zoomInOnScreen.RebuildGraph();
                _zoomInOnScreen.Play();
            }
        }
    }
}
