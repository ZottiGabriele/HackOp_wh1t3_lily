using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(PlayerInput)), RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] float _movementSpeed = 1.2f;
    [SerializeField] float _runningSpeedMultiplyer = 2;
    [SerializeField] float _jumpSpeed = 4f;

    public static bool ShouldLoadPosition = false;

    Vector2 _velocity = Vector2.zero;
    Animator _animator;
    Rigidbody2D _rigidBody;
    PlayerInput _playerInput;
    AudioSource _audioSource;
    bool _isRunning = false;
    bool _shouldInteract = true;
    bool _wasInputActive;

    private void Awake()
    {

        if (!Instance)
        {
            Instance = this;
            // DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Debug.LogWarning("ATTENTION: " + this + " has been destroyed because of double singleton");
            Destroy(this);
        }
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _audioSource = GetComponent<AudioSource>();

        GameStateHandler.Instance.OnGameStateChanged += onGameStateChanged;

        if (ShouldLoadPosition) LoadPlayerPos();

        onGameStateChanged(GameStateHandler.Instance.CurrentGameState);
    }

    private void OnDestroy()
    {
        GameStateHandler.Instance.OnGameStateChanged -= onGameStateChanged;
    }

    private void onGameStateChanged(GameStateHandler.GameState gameState)
    {
        switch (gameState)
        {
            case GameStateHandler.GameState.Paused:
            case GameStateHandler.GameState.UnpausableCutscene:
            case GameStateHandler.GameState.InteractingWithComputer:
                DisableInput();
                break;
            default:
                EnableInput();
                break;
        }
    }

    private void FixedUpdate()
    {
        if (!_shouldInteract) return;

        Vector2 translation = transform.right * _velocity.x * _movementSpeed * Time.fixedDeltaTime;
        // if (_isRunning) translation *= _runningSpeedMultiplyer;
        translation *= _runningSpeedMultiplyer;
        transform.Translate(translation);
        _animator.SetBool("is_jumping", Mathf.Abs(_rigidBody.velocity.y) >= 0.01f);
        // _animator.SetBool("is_running", _isRunning);
        _animator.SetBool("is_running", true);
    }

    public void DisableInput()
    {
        _wasInputActive = _playerInput.inputIsActive;
        _playerInput.DeactivateInput();
        _shouldInteract = false;
    }

    public void EnableInput()
    {
        _playerInput.ActivateInput();
        _shouldInteract = true;
    }

    //This method is used by the in game menu to restore input as before pausing
    public void RestoreInput()
    {
        if (_wasInputActive)
        {
            EnableInput();
        }
        else
        {
            DisableInput();
        }
    }

    public void LoadPlayerPos()
    {
        var pos = GameStateHandler.Instance.GameData.PlayerPosition;
        transform.position = new Vector3(pos[0], pos[1], pos[2]);
        ShouldLoadPosition = false;
    }

    public void OnHintTokenFound()
    {
        _animator.SetTrigger("hint_token_found");
    }

    // private void OnJump()
    // {
    //     if (Mathf.Abs(_rigidBody.velocity.y) <= 0.01f) _rigidBody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
    // }

    private void OnMove(InputValue inputValue)
    {
        if (!_shouldInteract) return;

        _velocity.x = inputValue.Get<float>();
        _animator.SetBool("is_moving", _velocity.x != 0);
        if (_velocity.x != 0) transform.localScale = new Vector3(_velocity.x, 1, 1);
    }

    // private void OnRunStart()
    // {
    //     _isRunning = true;
    // }

    // private void OnRunStop()
    // {
    //     _isRunning = false;
    // }

    private void OnInteract()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var hit = Physics2D.Raycast(mousePos + Vector3.forward, -Vector3.forward, 20, LayerMask.GetMask("Interaction"));

        if (hit.transform != null)
        {
            var areas = hit.transform.GetComponents<IInteractionArea>();
            foreach (var a in areas)
            {
                a.OnInteraction();
            }
        }
    }

    private void OnToggleMenu()
    {
        GeneralUIHandler.Instance.ToggleMenu();
    }

    public void PlayStepSound()
    {
        _audioSource.volume = SoundsHandler.Instance.SfxVolume;
        _audioSource.Play();
    }
}
