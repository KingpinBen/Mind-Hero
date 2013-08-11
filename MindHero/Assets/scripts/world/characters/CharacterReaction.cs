using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CharacterReaction
{
    public enum ReactionAnimation
    {
        None = -1,
        CelebrationOne, 
        CelebrationTwo, 
        CelebrationThree,
        MiseryOne, MiseryTwo, MiseryThree
    }

    public string message;
    public ReactionAnimation animation = ReactionAnimation.None;
}