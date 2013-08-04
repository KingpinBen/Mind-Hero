using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class WorldTrigger : MonoBehaviour {

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, .3f, .3f) * 0.7f;
        var boxCollider = collider as BoxCollider;
        var mat = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

        Gizmos.matrix = mat;
        Gizmos.DrawCube(boxCollider.center, boxCollider.size);
    }

    protected virtual void OnTriggerEnter(Collider body)
    {
        
    }
}
