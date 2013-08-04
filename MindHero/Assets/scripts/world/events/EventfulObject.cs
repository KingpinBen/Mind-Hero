using UnityEngine;
using System.Collections;

public class EventfulObject : MonoBehaviour
{

    protected bool _eventActive;

    public virtual void StartObject()
    {
        _eventActive = true;
    }

    public virtual void StopObject()
    {
        _eventActive = false;
    }

    public virtual void ToggleObject()
    {
        if (_eventActive) 
            StopObject();
        else 
            StartObject();
    }

    /// <summary>
    /// This will just be used to fire off something in any child events
    /// that aren't required to alter the objects state
    /// </summary>
    public virtual void FireObject()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, Vector3.one * .5f);
    }
}
