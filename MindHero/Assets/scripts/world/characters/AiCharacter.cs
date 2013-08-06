using UnityEngine;
using System.Collections;

public class AiCharacter : Character
{

    public bool followPlayerOnPass;
    public CharacterReaction passReaction = new CharacterReaction(true);
    public CharacterReaction failReaction = new CharacterReaction(false);
    public Vector2 followingOffset;

    private FollowerCrowdScript _crowdScript;
    private float _movingSpeed;
    private AiCharacterStatus _characterStatus = AiCharacterStatus.Idle;

    protected void Start()
    {
        _crowdScript = GameObject.FindWithTag("MainCamera").GetComponent<FollowerCrowdScript>();
    }

    protected void Update()
    {
        switch (_characterStatus)
        {
            case AiCharacterStatus.Idle:

                break;
            case AiCharacterStatus.Following:
                var dir = _crowdScript.followTarget.transform.position - transform.position;
                dir.y = 0;
                dir.z += followingOffset.y;
                dir.x += followingOffset.x;

                var lookAt = Quaternion.LookRotation(dir);
                transform.rotation = lookAt;
			
                if (followPlayerOnPass)
                {
                    _movingSpeed = Mathf.Lerp(_movingSpeed, _crowdScript.followTarget.GetMovementSpeed(), Time.deltaTime);

                    var speed = _crowdScript.followTarget.GetMovementSpeed()*dir.normalized.x;

                    _animator.SetFloat(_hashes.speed, speed);
                }
                break;
            case AiCharacterStatus.Retreating:
                transform.rotation = Quaternion.LookRotation(new Vector3(-1, 0, 0));
                break;
        }
    }

    public void CompletedBlockGroup(bool wasSuccessful)
    {
        _characterStatus = wasSuccessful ? 
            AiCharacterStatus.Following : AiCharacterStatus.Retreating;
        _crowdScript.AddFollower(wasSuccessful, this);

        if (wasSuccessful)
            _animator.SetInteger(_hashes.reaction, (int)passReaction.animation);
        else
            _animator.SetInteger(_hashes.reaction, (int)failReaction.animation);


        _animator.SetBool(_hashes.doReaction, true);
        StartCoroutine(StopReacting());
       
    }

    void OnBecameInvisible()
    {
        if (_characterStatus == AiCharacterStatus.Retreating)
            gameObject.SetActive(false);
    }

    IEnumerator StopReacting()
    {
        yield return new WaitForSeconds(.5f);

        _animator.SetBool(_hashes.doReaction, false);
    }
	
    /// <summary>
    /// Force the character to stop following the player.
    /// </summary>
    public void ForceFailure()
    {
        _characterStatus = AiCharacterStatus.Retreating;
        _animator.SetFloat(_hashes.speed, _crowdScript.followTarget.GetMovementSpeed());

        //  May want to force some sort of reaction.
    }

    internal enum AiCharacterStatus
    {
        Idle,
        Following,
        Retreating
    }
}
