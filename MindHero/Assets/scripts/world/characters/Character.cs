using UnityEngine;
using System.Collections;

[RequireComponent( typeof ( CharacterController ) )]
[RequireComponent( typeof ( Animator ) )]
[RequireComponent( typeof ( Rigidbody ) )]
[RequireComponent( typeof ( HashIDs ) )]
public class Character : MonoBehaviour
{
    protected float _targetMovementSpeed;
    protected Animator _animator;
    protected CharacterController _characterController;
    protected HashIDs _hashes;

    protected virtual void Awake()
    {
        _animator = GetComponent< Animator >();
        _characterController = GetComponent< CharacterController >();
        _hashes = GetComponent< HashIDs >();
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
