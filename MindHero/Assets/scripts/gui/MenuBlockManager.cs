using UnityEngine;
using System.Collections;

public class MenuBlockManager : MonoBehaviour {

    public MenuBlockFader[] faders;
    public float timeBetweenPings = 1.0f;

    private float _elapsed;

    void Update()
    {
        _elapsed += Time.deltaTime;

        if (_elapsed >= timeBetweenPings) StartNewPing();
    }

    void StartNewPing()
    {
        faders[Random.Range(0, 7)].Ping();
        _elapsed = 0.0f;
    }
}
