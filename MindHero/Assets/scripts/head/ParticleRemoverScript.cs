using UnityEngine;
using System.Collections;

public class ParticleRemoverScript : MonoBehaviour
{

    private float _elapsed;
	
	void Update ()
	{
	    _elapsed += Time.deltaTime;

        if (_elapsed >= 1.5f)
        {
            Destroy(gameObject);
        }
	}
}
