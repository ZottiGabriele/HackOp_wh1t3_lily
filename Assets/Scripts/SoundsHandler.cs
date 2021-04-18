using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundsHandler : MonoBehaviour
{
    public static SoundsHandler Instance { get; private set; }

    [SerializeField] AudioClip _bgMusic;
    [SerializeField] AudioClip _hintTokenFound;
    [SerializeField] AudioClip _text;
    [SerializeField] AudioClip _door;
    [SerializeField] AudioClip _keyPadBtn;
    [SerializeField] AudioClip _keyPadRight;
    [SerializeField] AudioClip _keyPadWrong;
    [SerializeField] bool playBgMusic = false;

    AudioSource _audioSource;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Debug.LogWarning("ATTENTION: " + this + " has been destroyed because of double singleton");
            Destroy(this);
        }
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (playBgMusic)
        {
            _audioSource.clip = _bgMusic;
            _audioSource.Play();
        }
    }

    public void PlayHintTokenFoundSound()
    {
        _audioSource.PlayOneShot(_hintTokenFound);
    }

    public void PlayTextSound()
    {
        _audioSource.PlayOneShot(_text);
    }

    public void PlayDoorSound()
    {
        _audioSource.PlayOneShot(_door);
    }

    public void PlayKeypadSound()
    {
        _audioSource.PlayOneShot(_keyPadBtn);
    }

    public void PlayKeypadRightSound()
    {
        _audioSource.PlayOneShot(_keyPadRight);
    }

    public void PlayKeypadWrongSound()
    {
        _audioSource.PlayOneShot(_keyPadWrong);
    }
}
