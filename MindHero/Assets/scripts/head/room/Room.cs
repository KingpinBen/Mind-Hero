using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[RequireComponent(typeof (BoxCollider))]
public class Room : MonoBehaviour
{
    public delegate void OnWorkerChangeAction();

    protected OnWorkerChangeAction _onWorkerChange;

    public event OnWorkerChangeAction OnWorkerChange
    {
        add { _onWorkerChange = (OnWorkerChangeAction) Delegate.Combine(_onWorkerChange, value); }
        remove { _onWorkerChange = (OnWorkerChangeAction) Delegate.Remove(_onWorkerChange, value); }
    }

    public RoomType roomType;
    public WorkerTask[] roomTasks;

    private readonly List<WorkerTaskInfo> _allTaskInfo = new List<WorkerTaskInfo>(); // Should remain untouched after initialized.
    protected readonly List<WorkerTaskInfo> _emptyTasks = new List<WorkerTaskInfo>();
    protected readonly List<WorkerInfo> _allWorkers = new List<WorkerInfo>();
    protected readonly List<WorkerInfo> _idleWorkers = new List<WorkerInfo>();

    protected virtual void Start()
    {
        for (var i = 0; i < roomTasks.Length; i++)
        {
            roomTasks[i].SetRoom(this);

            _allTaskInfo.Add(new WorkerTaskInfo(roomTasks[i], i));
            _emptyTasks.Add(_allTaskInfo[i]);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Worker") return;

        var worker = other.GetComponent<Worker>();
        var info = GetWorkerInfo(worker);

        if (_emptyTasks.Count > 0)
            DistributeTask(info);
        else
        {
            _idleWorkers.Add(info);
            info.worker.SetStatus(Worker.WorkerStatus.Idle);
        }

        if (_onWorkerChange != null)
            _onWorkerChange();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.tag != "Worker") return;

        var worker = other.GetComponent<Worker>();
        var info = GetWorkerInfo(worker);

        _allWorkers.Remove(info);

        //  If the removed worker was doing a task, cancel them doing it and 
        //  check if theres an idle worker to take their place.
        if (info.taskIndex >= 0)
        {
            _emptyTasks.Add(_allTaskInfo[info.taskIndex]);

            if (_idleWorkers.Count > 0)
            {
                DistributeTask(_idleWorkers[0]);
                _idleWorkers.RemoveAt(0);
            }
        }
        else
            _idleWorkers.Remove(info);

        worker.transform.parent = null; //  We null it so we know it no longer belongs to a room.


        if (_onWorkerChange != null)
            _onWorkerChange();
    }

    /// <summary>
    /// Give the Worker a new task in a room.
    /// This should only ever be called once we know for sure that
    /// there is an empty task slot for that room.
    /// </summary>
    /// <param name="workerInfo">WorkerInfo</param>
    protected void DistributeTask(WorkerInfo workerInfo)
    {
        var last = _emptyTasks.Count - 1;
        var taskInfo = _emptyTasks[last];

        workerInfo.taskIndex = taskInfo.roomTaskIndex;
        workerInfo.worker.GiveTask(taskInfo.task);
        workerInfo.worker.SetStatus(Worker.WorkerStatus.Busy);

        //  If the task can have more than one worker at a time (waypoint)
        //  we don't need to remove it (say it's taken)
        if (_emptyTasks[last].task.singleUser)
            _emptyTasks.RemoveAt(last);
    }

    /// <summary>
    /// Gets the WorkerInfo associated with the Worker for this room.
    /// If none exists, a new one is created and returned.
    /// </summary>
    /// <param name="worker">Who to check for</param>
    /// <returns>The WorkerInfo for the passed Worker</returns>
    protected WorkerInfo GetWorkerInfo(Worker worker)
    {
        for (var i = 0; i < _allWorkers.Count; i++)
            if (_allWorkers[i].worker == worker)
                return _allWorkers[i];

        //  It's a new Worker in the room so generate an 
        //  'information card' about it.
        var newInfo = new WorkerInfo(worker, -1);

        worker.GiveTask(null);
        worker.transform.parent = transform;

        _allWorkers.Add(newInfo);
        return newInfo;
    }

    /// <summary>
    /// Forces a 'random' worker to be removed from the room.
    /// Used by the block chart when the player successfully removed
    /// a block.
    /// </summary>
    /// <returns>True if a worker was removed.</returns>
    public bool RemoveWorker()
    {
        if (_allWorkers.Count == 0) return false;

        WorkerInfo info;

        if (_idleWorkers.Count > 0)
        {
            info = _idleWorkers[0];
            info.worker.DeactivateWorker();

            _allWorkers.Remove(info);
            _idleWorkers.RemoveAt(0);
        }
        else
        {
            info = _allWorkers[0];

            info.worker.DeactivateWorker();
            _emptyTasks.Add(_allTaskInfo[info.taskIndex]);

            _allWorkers.RemoveAt(0);
        }

        if (_onWorkerChange != null) 
            _onWorkerChange();

        return true;
    }

    public int GetWorkerCount()
    {
        return _allWorkers.Count;
    }

    void OnDrawGizmos()
    {
        var box = GetComponent<BoxCollider>();
        Gizmos.color = Color.black*0.6f;
        Gizmos.DrawCube(transform.position + box.center, box.size);
    }
}
