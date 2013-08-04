using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CharacterReaction
{
    public enum ReactionAnimation
    {
        CelebrationOne, CelebrationTwo, CelebrationThree,
        MiseryOne, MiseryTwo, MiseryThree
    }

    public string message;
    public ReactionAnimation animation;

    public CharacterReaction(bool val)
    {
        animation = val ? ReactionAnimation.CelebrationOne : ReactionAnimation.MiseryOne;
    }
}