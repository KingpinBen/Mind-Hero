using UnityEngine;
using System.Collections;

/// <summary>
/// Immediately fires off any objects it's connected too upon being
/// touched by the player object.
/// </summary>
public class EventfulTrigger : WorldTrigger
{

    public EventfulObject[] outputObjects;

    protected override void OnTriggerEnter(Collider body)
    {
        base.OnTriggerEnter(body);

        if (body.tag == "WorldPlayer")
        {
            for(var i = 0; i < outputObjects.Length; i++)
                outputObjects[i].ToggleObject();
        }
    }

    void OnDrawGizmosSelected()
    {
        for (var i = 0; i < outputObjects.Length; i++)
            if (outputObjects[i])
                Gizmos.DrawLine(transform.position, outputObjects[i].transform.position);
    }
}
