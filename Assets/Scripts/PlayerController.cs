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

    Vector2 _velocity = Vector2.zero;
    Animator _animator;
    Rigidbody2D _rigidBody;
    PlayerInput _playerInput;
    AudioSource _audioSource;
    bool _isRunning = false;
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
    }


    private void FixedUpdate()
    {
        Vector2 translation = transform.right * _velocity.x * _movementSpeed * Time.fixedDeltaTime;
        if (_isRunning) translation *= _runningSpeedMultiplyer;
        transform.Translate(translation);
        _animator.SetBool("is_jumping", Mathf.Abs(_rigidBody.velocity.y) >= 0.01f);
        _animator.SetBool("is_running", _isRunning);
    }

    public void DisableInput()
    {
        _wasInputActive = _playerInput.inputIsActive;
        _playerInput.DeactivateInput();
    }

    public void EnableInput()
    {
        _playerInput.ActivateInput();
    }

    //This method is used by the in game menu to restore input as before pausing
    public void RestoreInput()
    {
        if (_wasInputActive)
        {
            _playerInput.ActivateInput();
        }
        else
        {
            _playerInput.DeactivateInput();
        }
    }

    public void OnHintTokenFound()
    {
        _animator.SetTrigger("hint_token_found");
    }

    private void OnJump()
    {
        if (Mathf.Abs(_rigidBody.velocity.y) <= 0.01f) _rigidBody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
    }

    private void OnMove(InputValue inputValue)
    {
        _velocity.x = inputValue.Get<float>();
        _animator.SetBool("is_moving", _velocity.x != 0);
        if (_velocity.x != 0) transform.localScale = new Vector3(_velocity.x, 1, 1);
    }

    private void OnRunStart()
    {
        _isRunning = true;
    }

    private void OnRunStop()
    {
        _isRunning = false;
    }

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

    public void PlayStepSound()
    {
        _audioSource.Play();
    }
}
