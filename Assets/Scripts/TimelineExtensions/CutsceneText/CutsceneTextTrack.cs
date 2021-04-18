using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;


/// <summary>
/// Bindings for a CutsceneTextTrack required for Timeline extension
/// </summary>

[TrackBindingType(typeof(GeneralUIHandler))]
[TrackClipType(typeof(CutsceneTextClip))]
public class CutsceneTextTrack : TrackAsset
{

}
