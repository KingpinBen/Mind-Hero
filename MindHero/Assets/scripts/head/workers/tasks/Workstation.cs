using UnityEngine;
using System.Collections;

public class Workstation : WorkerTask
{
    public WorkstationScreenScript screen;

    private void Awake()
    {
        var targetPos = transform.position;
        targetPos.z = 0;

        transform.position = targetPos;
    }

    public override void SetInUse(bool use)
    {
        base.SetInUse(use);
        screen.ToggleScreen(use);
    }
}
