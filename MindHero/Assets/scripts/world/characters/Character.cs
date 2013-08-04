using UnityEngine;
using System.Collections;

[RequireComponent(typeof (CharacterController))]
[RequireComponent(typeof (Animator))]
[RequireComponent(typeof (Rigidbody))]
public class Character : MonoBehaviour
{
    protected float _targetMovementSpeed;
    protected Animator _animator;
    protected CharacterController _characterController;

    protected virtual void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    protected virtual void Update()
    {

    }

    protected bool IsGrounded()
    {
        return _characterController.isGrounded;
    }

    public float GetMovementSpeed()
    {
        return _targetMovementSpeed;
    }
}
