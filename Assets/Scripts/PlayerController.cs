using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float _movementSpeed = 1.2f;
    [SerializeField] float _runningSpeedMultiplyer = 2;
    [SerializeField] float _jumpSpeed = 4f;

    Vector2 _velocity = Vector2.zero;
    Animator _animator;
    Rigidbody2D _rigidBody;
    bool _isRunning = false;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnJump() {
        if(Mathf.Abs(_rigidBody.velocity.y ) <= 0.01f) _rigidBody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
    }

    private void OnMove(InputValue inputValue) {
        _velocity.x = inputValue.Get<float>();
        _animator.SetBool("is_moving", _velocity.x != 0);
        if(_velocity.x != 0) transform.localScale = new Vector3(_velocity.x,1,1);
    }

    private void OnRun() {
        _isRunning = !_isRunning;
        _animator.SetBool("is_running", _isRunning);
    }

    private void FixedUpdate() {
        Vector2 translation = transform.right * _velocity.x * _movementSpeed * Time.fixedDeltaTime;
        if(_isRunning) translation *= _runningSpeedMultiplyer;
        transform.Translate(translation);
        _animator.SetBool("is_jumping", Mathf.Abs(_rigidBody.velocity.y ) >= 0.01f);
    }
}
