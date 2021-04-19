using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundsHandler : MonoBehaviour
{
    public static SoundsHandler Instance { get; private set; }

    public float BgMusicVolume { get => _bgMusicVolume; set { _bgMusicVolume = value; _musicAudioSource.volume = _bgMusicVolume; } }
    public float SfxVolume { get => _sfxVolume; set { _sfxVolume = value; _sfxAudioSource.volume = _sfxVolume; } }

    [SerializeField] AudioSource _musicAudioSource;
    [SerializeField] AudioSource _sfxAudioSource;
    [SerializeField] AudioClip _bgMusic;
    [SerializeField] AudioClip _hintTokenFound;
    [SerializeField] AudioClip _text;
    [SerializeField] AudioClip _door;
    [SerializeField] AudioClip _keyPadBtn;
    [SerializeField] AudioClip _keyPadRight;
    [SerializeField] AudioClip _keyPadWrong;
    [SerializeField] bool playBgMusic = false;

    float _bgMusicVolume = 1;
    float _sfxVolume = 1;


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
        if (playBgMusic)
        {
            _musicAudioSource.clip = _bgMusic;
            _musicAudioSource.volume = BgMusicVolume;
            _musicAudioSource.Play();
        }

        _sfxAudioSource.volume = SfxVolume;
    }

    public void PlayHintTokenFoundSound()
    {
        _sfxAudioSource.PlayOneShot(_hintTokenFound);
    }

    public void PlayTextSound()
    {
        _sfxAudioSource.PlayOneShot(_text);
    }

    public void PlayDoorSound()
    {
        _sfxAudioSource.PlayOneShot(_door);
    }

    public void PlayKeypadSound()
    {
        _sfxAudioSource.PlayOneShot(_keyPadBtn);
    }

    public void PlayKeypadRightSound()
    {
        _sfxAudioSource.PlayOneShot(_keyPadRight);
    }

    public void PlayKeypadWrongSound()
    {
        _sfxAudioSource.PlayOneShot(_keyPadWrong);
    }
}
