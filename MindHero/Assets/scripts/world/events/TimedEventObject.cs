using UnityEngine;
using System.Collections;

public class TimedEventObject : EventfulObject
{
    public float timeBeforeFire = 1.0f;
    public EventfulObject[] outputObjects;
	
    public override void StartObject()
    {
        base.StartObject();
        StartCoroutine(TimedEventHandler());
    }

    private IEnumerator TimedEventHandler()
    {
        yield return new WaitForSeconds(timeBeforeFire);

        for (var i = 0; i < outputObjects.Length; i++)
            outputObjects[i].ToggleObject();

        StopObject();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = _eventActive ? Color.green : Color.red;
        Gizmos.DrawCube(transform.position, Vector3.one *.5f);
    }
}
