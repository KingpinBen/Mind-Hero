using UnityEngine;
using System.Collections;

public class WorldCameraScript : MonoBehaviour {

    public Transform cameraTarget;
    public float rightOffset = 0.0f;
    public float movementSpeed = 1.0f;

    void Update () {
        if (!cameraTarget) return;
        var target = transform.position;

        /*
         * Only need to move along the X. We'll lerp it so it doesn't jolt.
         * TODO: Depending on the future plans, we may need to move Y too.
        */
        target.x += ((rightOffset + cameraTarget.position.x) - target.x) * Time.deltaTime * movementSpeed;
        transform.position = target;
    }
}
