using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HashIDs))]
public class AiCharacter : Character
{

    public bool followPlayerOnPass;
    public CharacterReaction passReaction;
    public CharacterReaction failReaction;
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
                dir.z -= followingOffset.y;
                dir.x -= followingOffset.x;

                transform.rotation = Quaternion.LookRotation(dir);
			
                if (followPlayerOnPass)
                {
                    _movingSpeed = Mathf.Lerp(_movingSpeed, _crowdScript.followTarget.GetMovementSpeed(), Time.deltaTime);

                    var speed = Mathf.Clamp(_movingSpeed*dir.normalized.x, 0, 1);

                    _animator.SetFloat(_hashes.speed, speed);
                }
                break;
            case AiCharacterStatus.Retreating:
                break;
        }
    }

    public void CompletedBlockGroup(bool wasSuccessful)
    {
        _characterStatus = wasSuccessful ? 
            AiCharacterStatus.Following : AiCharacterStatus.Retreating;
        _crowdScript.AddFollower(wasSuccessful, this);

        var reactionToPlay = wasSuccessful ? (int)passReaction.animation : (int)failReaction.animation;

        if (reactionToPlay > 0)
        {
            _animator.SetInteger( _hashes.reaction, reactionToPlay );
            _animator.SetBool(_hashes.doReaction, true);

            StartCoroutine(StopReacting());
        }      
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

    private enum AiCharacterStatus
    {
        Idle,
        Following,
        Retreating
    }
}
