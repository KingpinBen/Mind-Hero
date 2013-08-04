using UnityEngine;
using System.Collections;

public class EyeBehaviourScript : MonoBehaviour
{

    public Transform pupil;

    private Vector3 _targetPositionForPupil = Vector3.zero;
    private float _timeBeforeChange;

    void Start()
    {
        LookAt(EyeLookAtPosition.Forward);
    }

	void Update ()
	{
	    pupil.localPosition = Vector3.Slerp(pupil.localPosition, _targetPositionForPupil, Time.deltaTime*5f);
	}

    IEnumerator ChangeLookAt()
    {
        yield return new WaitForSeconds(_timeBeforeChange);

        var lookAt = (EyeLookAtPosition) Random.Range(0, 3);

        LookAt(lookAt, Random.Range(0.75f, 2.5f));
    }

    public void LookAt(EyeLookAtPosition lookAt)
    {
        LookAt(lookAt, 2.5f);
    }

    public void LookAt(EyeLookAtPosition lookAt, float stareTime)
    {
        switch (lookAt)
        {
            case EyeLookAtPosition.Ground:
                _targetPositionForPupil = new Vector3(.2f, -.25f, .2f);
                break;
            case EyeLookAtPosition.Forward:
                _targetPositionForPupil = new Vector3(.25f, 0, .2f);
                break;
            case EyeLookAtPosition.Up:
                _targetPositionForPupil = new Vector3(.15f, .15f, .2f);
                break;
            case EyeLookAtPosition.Back:
                _targetPositionForPupil = new Vector3(-.25f, -.05f, .2f);
                break;
            case EyeLookAtPosition.Side:
                _targetPositionForPupil = new Vector3(.07f, 0, .2f);
                break;
        }

        _timeBeforeChange = stareTime;
		
		//	Stop all existing coroutines as it gets called
		//	whenever a reaction is pushed through for the head.
		StopAllCoroutines();
        StartCoroutine(ChangeLookAt());
    }

    public enum EyeLookAtPosition
    {
        Forward, Side,
        Up, Back, Ground
    }
}
