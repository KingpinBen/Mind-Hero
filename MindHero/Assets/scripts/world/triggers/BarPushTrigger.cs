using UnityEngine;
using System;

/// <summary>
/// Just being used for a serializable 2D array.
/// </summary>
[Serializable]
public class BlockColumnArray
{
    public bool[] rows = new bool[6];   //  The amount of coloured rooms

    public bool this[int index]
    {
        get { return rows[index]; }
        set { rows[index] = value; }
    }

    public BlockColumnArray()
    {
    }
}

public class BarPushTrigger : WorldTrigger
{

    public BlockColumnArray[] columns = new BlockColumnArray[1];
    public AiCharacter character;
    public string message;

    private BarManager _barManager;
    private readonly CharacterData _characterData = new CharacterData();

    private void Start()
    {
        _barManager = GameObject.FindWithTag
            ("GameController").GetComponent<BarManager>();

        _characterData.character = character;

        var c = 0;
        var r = 0;

        //  Like new-lining a typewriter. Finish row, do new line.
        for (; r < columns[0].rows.Length; r++)
        {
            for (c = 0; c < columns.Length; c++)
            {
            	if (columns[c].rows[r])
                    _characterData.blockCount++;
            }
        }
    }

    protected override void OnTriggerEnter(Collider body)
    {
        if (body.tag != "WorldPlayer") 
            return;

        _barManager.ReceivePush(columns, _characterData);
        GameObject.FindWithTag("MainCamera").GetComponent<FollowerCrowdScript>().CreateMessage(message);
    }
}
