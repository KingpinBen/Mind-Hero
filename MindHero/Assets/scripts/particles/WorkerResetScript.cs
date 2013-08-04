using UnityEngine;
using System.Collections;

public class WorkerResetScript : MonoBehaviour
{

    private ParticleSystem _particleComp;
    private float _elapsed;

    private void Awake()
    {
        _particleComp = GetComponent<ParticleSystem>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (_particleComp.isStopped)
        {
            Destroy(gameObject);
        }
    }
}
