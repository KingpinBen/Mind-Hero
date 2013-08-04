using UnityEngine;
using System.Collections;

public class BlockParticlePop : MonoBehaviour
{

    private AudioSource _as;

    void Start()
    {
        particleSystem.Stop();
        _as = GetComponent<AudioSource>();
    }

    public void Pop()
    {
        particleSystem.Play();
        _as.Play();
    }
}
