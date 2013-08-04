using UnityEngine;
using System.Collections;

public class PlayerSpeedTrigger : WorldTrigger
{
    public float speed;

    private PlayerCharacter _player;

    void Start()
    {
        _player = GameObject.FindWithTag("WorldPlayer").GetComponent<PlayerCharacter>();
    }

    protected override void OnTriggerEnter(Collider body)
    {
        if (body.tag == "WorldPlayer")
            _player.ChangeSpeed(speed);
    }
}
