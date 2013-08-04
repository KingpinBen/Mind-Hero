using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraParallaxScript : MonoBehaviour
{

    public ParallaxObject[] parallaxObjects = new ParallaxObject[0];

    private Vector3 _oldCameraXPosition;
    private Camera _worldCamera;

    private void Start()
    {
        _worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        _oldCameraXPosition = _worldCamera.transform.position;
    }

    private void Update()
    {
        var newCameraPosition = _worldCamera.transform.position;
        var difference = _oldCameraXPosition - newCameraPosition;

        Vector3 localPosition;

        for (var i = 0; i < parallaxObjects.Length; i++)
        {
            localPosition = parallaxObjects[i].parallaxObject.transform.localPosition;
            localPosition += difference*parallaxObjects[i].offsetAmount;
            parallaxObjects[i].parallaxObject.transform.localPosition = localPosition;

            //localPosition.x -= difference*parallaxObjects[i].offsetAmount;

            //parallaxObjects[i].parallaxObject.transform.position = localPosition;
        }

        _oldCameraXPosition = newCameraPosition;
    }
}
