using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CharacterData {
	public float score;
	public int missedBlocks;
    public int correctBlocks;
    public int blockCount;
	public AiCharacter character;
	
	public bool CheckComplete() 
	{
		if ((missedBlocks + correctBlocks) >= blockCount)
		{
            score = (float)correctBlocks / (float)blockCount;
		    character.CompletedBlockGroup(score >= 0.9f);
		    return true;
		}

	    return false;
	}
}
