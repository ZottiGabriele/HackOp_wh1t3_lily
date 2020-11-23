using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class Cutscene : IGameDataObserver
{
    [SerializeField] bool _playOnStart;
    PlayableDirector _pd;

    private void Awake() {
        _pd = GetComponent<PlayableDirector>();
    }

    private void Start() {
        if(_playOnStart) Play();
    }

    private void OnDestroy() {
    }

    public void Play() {
        executeOnCondition();
    }

    public void Play(Action<bool> onCutsceneEnd) {
        bool hasExecuted = executeOnCondition();
        Action<PlayableDirector> tmp = null;
        tmp = _ => {
            onCutsceneEnd(hasExecuted); 
            _pd.stopped -= tmp; 
        };
        _pd.stopped += tmp;
    }

    public void ForcePlay() {
        execute();
    }

    public void ForcePlay(Action onCutsceneEnd) {
        execute();
        Action<PlayableDirector> tmp = null;
        tmp = _ => {
            onCutsceneEnd(); 
            _pd.stopped -= tmp; 
        };
        _pd.stopped += tmp;
    }

    protected override void execute()
    {
        _pd.Play();
    }
}
