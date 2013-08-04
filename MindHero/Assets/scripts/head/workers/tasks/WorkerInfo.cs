using UnityEngine;
using System.Collections;
using System;

public class WorkerInfo
{
    public Worker worker;
    public int taskIndex;

    public WorkerInfo(Worker worker, int index)
    {
        this.worker = worker;
        this.taskIndex = index;
    }
}
