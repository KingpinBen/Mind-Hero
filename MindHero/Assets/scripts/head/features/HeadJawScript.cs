using UnityEngine;
using System.Collections;

public class HeadJawScript : MonoBehaviour
{

    private float _timeToStayOpen = 0.0f;
    private int _chatterLeft;
    private bool _open;
    private const float ROTATION_SPEED_MULTIPLIER = 30.0f;
    private bool _waiting;

    private void Update()
    {
        if (_waiting) return;

        //  '!_open' is added at the end to make sure it runs once
        //  extra to close the mouth as it will be open on 0
        if (_chatterLeft > 0 || !_open)
        {
            var euler = transform.rotation.eulerAngles;
            if (_open)
            {
                if (euler.z <= 352 && euler.z > 10f)
                {
                    euler.z = 352;
                    _waiting = true;
                    StartCoroutine(ToggleMouthState());
                }
                else
                    euler.z -= Time.deltaTime * ROTATION_SPEED_MULTIPLIER;
            }
            else
            {
                if (euler.z >= 0.1f && euler.z < 10f)
                {
                    euler.z = 0.0f;
                    _waiting = true;
                    StartCoroutine(ToggleMouthState());
                }
                else
                    euler.z += Time.deltaTime * ROTATION_SPEED_MULTIPLIER;
            }

            transform.rotation = Quaternion.Euler(euler);
        }
    }

    IEnumerator ToggleMouthState()
    {
        yield return new WaitForSeconds(_timeToStayOpen);

        _open = !_open;
        _waiting = false;

        if (_chatterLeft > 0 && !_open)
            _chatterLeft--;
    }

    /// <summary>
    /// Causes the mouth to open and close repeatedly
    /// </summary>
    /// <param name="amountToOpen">Amount of times to open</param>
    public void CreateChatter(int amountToOpen)
    {
        if (_chatterLeft > 0) return;

        _chatterLeft = amountToOpen;
        _timeToStayOpen = 0.1f;
        _waiting = false;

        StopAllCoroutines();
    }

    /// <summary>
    /// Makes Brian open his mouth in shock 
    /// over what just happened.
    /// </summary>
    public void CreateShock()
    {
        if (_chatterLeft > 0) return;

        _chatterLeft = 1;
        _timeToStayOpen = 3.0f;
        _open = true;
        StopAllCoroutines();
        _waiting = false;
    }

    public void StartSick()
    {
        _chatterLeft = 1;
        _timeToStayOpen = 5.3f;
        _open = true;
        _waiting = false;

        StopAllCoroutines();
    }
}
