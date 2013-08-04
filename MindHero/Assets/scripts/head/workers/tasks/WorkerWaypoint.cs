using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class WorkerWaypoint : WorkerTask
{

    public WorkerWaypoint nextWaypoint;
    public float pointDelay = 1.0f;

    private readonly List<Worker> _workersInRange = new List<Worker>();

	void Start () {
	    
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Worker") return;

        _workersInRange.Add(other.GetComponent<Worker>());
        StartCoroutine(SendToNextWaypoint());
    }

    IEnumerator SendToNextWaypoint()
    {
        yield return new WaitForSeconds(pointDelay);

        var worker = _workersInRange[0];

        if (worker.GetTask() == this)
        {
            if (_workersInRange.Count > 0)
            {
                _workersInRange[0].GiveTask(nextWaypoint);
                _workersInRange.RemoveAt(0);
            }
        }
        else
        {
            _workersInRange.Remove(worker);
        }
        
    }
}
