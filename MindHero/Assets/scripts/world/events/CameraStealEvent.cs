using UnityEngine;
using System.Collections;

public class CameraStealEvent : EventfulObject
{

    public Transform newCameraTarget;

    private WorldCameraScript _worldCamera;
    private Transform _oldTarget;

    private void Start()
    {
        if ( !newCameraTarget )
        {
            Debug.Log( "[" + GetType() + "]: " + name + " doesn't have a target to switch to when triggered!" );
            gameObject.SetActive( false );
            return;
        }

        _worldCamera = GameObject.FindWithTag( "MainCamera" ).GetComponent< WorldCameraScript >();
    }

    /// <summary>
    /// Will toggle the cameras target between the newCameraTarget 
    /// member transform and the previous cameraTarget if 
    /// newCameraTarget is already active.
    /// </summary>
    public override void ToggleObject()
    {
        FireObject();
    }

    public override void FireObject()
    {
        if ( !newCameraTarget )
            return;

        var currentTarget = _worldCamera.cameraTarget;

        if ( currentTarget == newCameraTarget )
            _worldCamera.cameraTarget = _oldTarget;
        else
        {
            _oldTarget = currentTarget;
            _worldCamera.cameraTarget = newCameraTarget;
        }
    }
}
