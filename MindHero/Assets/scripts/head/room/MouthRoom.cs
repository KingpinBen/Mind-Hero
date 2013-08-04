using System;
using UnityEngine;
using System.Collections;

public class MouthRoom : WildcardRoom
{
    public ParticleSystem spewSystem;

    private FollowerCrowdScript _crowdScript;
    private bool _spewing;

    protected virtual void Awake()
    {
        _crowdScript = GameObject.FindWithTag("MainCamera").GetComponent<FollowerCrowdScript>();
    }

    protected override void Start()
    {
        base.Start();
        spewSystem.Stop();
    }

    protected override void Update()
    {
        if (_spewing)
        {
            if (spewSystem.isStopped)
            {
                if (roomScoreRaw >= 0)
                {
                    _spewing = false;
                    roomScoreRaw = 0;
                }
                else
                {
                    roomScoreRaw = Time.deltaTime;
                }
            }
        }
        else
        {
            base.Update();

            if (roomScore01 == 0)
                StartSpewing();
        }
    }

    void StartSpewing()
    {
        _crowdScript.LoseFollower();

        _spewing = true;
        _crowdScript.GetHead().jaw.StartSick();
        spewSystem.Play();
        StartCoroutine(StopSpewing());
    }

    IEnumerator StopSpewing()
    {
        yield return new WaitForSeconds(2.5f);

        spewSystem.Stop();
    }
}
