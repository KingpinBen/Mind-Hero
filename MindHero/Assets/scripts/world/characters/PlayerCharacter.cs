using UnityEngine;

public class PlayerCharacter : Character
{
    private float _currentSpeed;

    protected override void Start()
    {
        base.Start();

        _currentSpeed = PlayerPrefs.GetFloat("startMovementSpeed");
        _animator.SetFloat("Speed", _currentSpeed);
    }

    protected override void Update()
    {
        if (Mathf.Abs(_currentSpeed - _targetMovementSpeed) > Mathf.Epsilon)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, _targetMovementSpeed, Time.deltaTime * 2.0f);
            _animator.SetFloat("Speed", _currentSpeed);
        }
    }

    public void ChangeSpeed(float targetSpeed)
    {
        _targetMovementSpeed = targetSpeed;
    }
}
