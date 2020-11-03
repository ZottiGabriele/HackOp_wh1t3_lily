using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class VisionRayRelayer : MonoBehaviour
{
    [SerializeField] PlayableDirector _gameOver;
    [SerializeField] string _caughtText;

    bool _triggered;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player") && !_triggered) {
            GameStateHandler.Instance.GameOver();
            GeneralUIHandler.Instance.ShowText(_caughtText);
            _triggered = true;
        }
    }

    public void RemoveTrigger() {
        _triggered = false;
    }
}
