using UnityEngine;
using System.Collections;

public class WorkerTask : MonoBehaviour {

    public Transform lookAtTarget;
    public bool singleUser = true;

    protected bool _workerOnTask;

    protected Room _room;

    public virtual void SetInUse(bool use)
    {
        _workerOnTask = use && singleUser;
    }

    public bool GetInUse()
    {
        return singleUser && _workerOnTask;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue*.7f;
        Gizmos.DrawSphere(transform.position, .5f);
    }

    public void SetRoom(Room room)
    {
        if (_room == false)
            _room = room;
    }
}
