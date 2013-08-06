using UnityEngine;

/// <summary>
/// An object that can be picked up by the player.
/// </summary>
public class GrabObject : MonoBehaviour {

    protected bool _grabbed;

    public virtual void Drop()
    {
        _grabbed = false;
    }

    public virtual void PickUp()
    {
        _grabbed = true;
    }

    public bool GetGrabbed()
    {
        return _grabbed;
    }
}
