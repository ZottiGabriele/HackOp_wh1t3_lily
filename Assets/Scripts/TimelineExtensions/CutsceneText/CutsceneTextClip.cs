using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// Timeline clip that wraps the CutsceneTextBehaviour required for Timeline extension
/// </summary>

[System.Serializable]
public class CutsceneTextClip : PlayableAsset, ITimelineClipAsset
{
    [SerializeField] CutsceneTextBehaviour _template = new CutsceneTextBehaviour();
    CutsceneTextBehaviour _behaviour;

    public ClipCaps clipCaps => ClipCaps.None;

    public override double duration
    {
        get
        {
            if (_behaviour == null) return base.duration;
            return _behaviour.GetClipDurationFromText();
        }
    }

    public override IEnumerable<PlayableBinding> outputs => base.outputs;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var x = ScriptPlayable<CutsceneTextBehaviour>.Create(graph, _template);
        _behaviour = x.GetBehaviour();
        return x;
    }
}

