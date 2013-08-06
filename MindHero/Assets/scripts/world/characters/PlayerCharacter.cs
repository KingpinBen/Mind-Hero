using UnityEngine;

public class PlayerCharacter : Character
{
    private float _currentSpeed;

    protected void Start()
    {
        _currentSpeed = PlayerPrefs.GetFloat( "startMovementSpeed" );
        _animator.SetFloat( _hashes.speed, _currentSpeed );
    }

    protected void Update()
    {
        if ( Mathf.Abs( _currentSpeed - _targetMovementSpeed ) > Mathf.Epsilon )
        {
            _currentSpeed = Mathf.Lerp( _currentSpeed, _targetMovementSpeed, Time.deltaTime * 2.0f );
            _animator.SetFloat(_hashes.speed, _currentSpeed);
        }
    }

    public void ChangeSpeed( float targetSpeed )
    {
        _targetMovementSpeed = targetSpeed;
    }
}
