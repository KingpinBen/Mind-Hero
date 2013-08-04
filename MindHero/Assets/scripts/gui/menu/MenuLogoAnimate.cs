using UnityEngine;
using System.Collections;

public class MenuLogoAnimate : MonoBehaviour
{

    public float maximumRotation;
    public float scaleModifier = 1.0f;
    public float scaleSpeedModifier;
    public float rotationSpeedModifier;

    private Transform _logoTransform;

	void Start ()
	{
	    _logoTransform = transform;
	}
	
	void Update ()
	{
	    var rotation = _logoTransform.rotation.eulerAngles;
	    var scale = _logoTransform.localScale;

	    rotation.z += Time.deltaTime*rotationSpeedModifier;

        if (rotation.z > 360 - maximumRotation) rotation.z -= 360;

        if (maximumRotation > 0)
        {
            if (rotation.z > maximumRotation % 360)
                rotationSpeedModifier = -rotationSpeedModifier;
        } 
        else
        {
            if (rotation.z < -maximumRotation % 360)
                rotationSpeedModifier = -rotationSpeedModifier;
        }

        _logoTransform.rotation = Quaternion.Euler(rotation);

	    var scaler = scale.x;
        
	    scaler += Time.deltaTime*scaleSpeedModifier;

        if (scaleSpeedModifier > 0)
        {
            if (scaler > scaleModifier) scaleSpeedModifier = -scaleSpeedModifier;
        } 
        else
        {
            if (scaler < 1.0f) scaleSpeedModifier = -scaleSpeedModifier;
        }

        _logoTransform.localScale = new Vector3(scaler, scaler, 1);
	}
}
