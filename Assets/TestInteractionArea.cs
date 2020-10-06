using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TestInteractionArea : IInteractionArea
{
    Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    public override void OnInteraction()
    {
        _animator.SetTrigger("OnInteraction");
    }
}
