using UnityEngine;
using System.Collections;

public class WorkstationScreenScript : MonoBehaviour {

    public void ToggleScreen(bool set)
    {
        renderer.material.color = set ? Color.blue : Color.red;
    }
}
