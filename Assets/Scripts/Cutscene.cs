using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class Cutscene : IGameDataObserver
{
    PlayableDirector _pd;

    private void Awake() {
        _pd = GetComponent<PlayableDirector>();    
    }

    public void Play() {
        executeOnCondition();
    }

    public void ForcePlay() {
        execute();
    }

    protected override void execute()
    {
        _pd.Play();
    }
}
