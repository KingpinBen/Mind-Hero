using UnityEngine;
using System.Collections;
using System;

public class WorkerTaskInfo
{
    public WorkerTask task;
    public int roomTaskIndex;

    public WorkerTaskInfo(WorkerTask task, int index)
    {
        this.task = task;
        roomTaskIndex = index;
    }
}
