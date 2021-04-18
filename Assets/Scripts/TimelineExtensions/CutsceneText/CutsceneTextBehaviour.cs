using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// Timeline behaviour that processes the given text and displays it on screen using the Timeline component.
/// This works also in Timeline's preview mode to clearly see how the text will be displayed at any given moment.
/// </summary>

[Serializable]
public class CutsceneTextBehaviour : PlayableBehaviour
{
    [SerializeField] string _text = "";
    private GeneralUIHandler _guih;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        _guih = playerData as GeneralUIHandler;

        if (_guih == null) return;

        int c_index = (int)(playable.GetTime() / (1f - _guih._typingSpeed));
        //_guih.StopAllCoroutines();
        _guih.PlayText((c_index < _text.Length) ? _text.Remove(c_index) : _text);
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (_guih == null) return;
        _guih.StopText();
    }

    public float GetClipDurationFromText()
    {
        if (_guih == null) return 1;
        return ((_text.Length + 2) * (1 - _guih._typingSpeed) + 2.5f);
    }
}
