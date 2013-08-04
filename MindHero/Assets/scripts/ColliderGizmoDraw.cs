using UnityEngine;
using System.Collections;

public class ColliderGizmoDraw : MonoBehaviour
{
	void Start ()
	{
	    
	}

    void OnDrawGizmos()
    {
        var col = collider as BoxCollider;

        var size = new Vector3(col.size.x*transform.localScale.x,
                               col.size.y*transform.localScale.y,
                               col.size.z * transform.localScale.z);

        Gizmos.color = Color.white*.5f;
        Gizmos.DrawCube(transform.position + col.center, size);
    }
}
