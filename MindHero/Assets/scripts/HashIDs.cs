using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour
{
    public int speed;
    public int reaction;
    public int doReaction;

    void Awake()
    {
        speed = Animator.StringToHash("Speed");
        reaction = Animator.StringToHash("Reaction");
        doReaction = Animator.StringToHash("DoReaction");
    }
}
