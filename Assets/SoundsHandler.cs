using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundsHandler : MonoBehaviour
{
    public static SoundsHandler Instance {get; private set;}
    
    [SerializeField] AudioClip _bgMusic;
    [SerializeField] AudioClip _hintTokenFound;
    [SerializeField] bool playBgMusic = false;

    AudioSource _audioSource;

    private void Awake() {
        if(!Instance) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else if (Instance != this) {
            Debug.LogWarning("ATTENTION: " + this + " has been destroyed because of double singleton");
            Destroy(this);
        }
    }

    private void Start() {
        _audioSource = GetComponent<AudioSource>();
        if(playBgMusic) {
            _audioSource.clip = _bgMusic;
            _audioSource.Play();
        }
    }

    public void PlayHintTokenFoundSound() {
        _audioSource.PlayOneShot(_hintTokenFound);
    }
}
