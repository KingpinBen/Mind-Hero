using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BarManager : MonoBehaviour
{
    public static BarManager Instance;

    public RoomBar[] bars = new RoomBar[7];
    public float blockSpeed = 1.0f;

    private float _elapsed;
    private float _pulseInterval;
    private WildcardRoom _eyeRoom;
    private BlockTextMover _textMesh;

    private readonly List< CharacterData > _activeCharacters = new List< CharacterData >();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        var head = GameObject.FindWithTag("BriansHead").GetComponent<HeadScript>();
        _eyeRoom = head.wildcardRooms[0];

        for (var i = 0; i < bars.Length; i++)
        {
            bars[i].ChangeBlockSpeed(blockSpeed);
            bars[i].manager = this;
        }

        //  Added .25f for a bit of separation between blocks.
        _pulseInterval = (1.25f/blockSpeed);

        StartCoroutine( SendBlockPulse() );
    }

    public void ReceivePush(BlockColumnArray[] blocks, CharacterData characterData)
    {
        if (blocks == null) 
            throw new ArgumentNullException("blocks");
        if (characterData == null) 
            throw new ArgumentNullException("characterData");

        _activeCharacters.Add(characterData);

        //  Size of the amount of columns
        var rowValues = new bool[blocks.Length];

        for (var r = 0; r < bars.Length; r++)
        {
            for (var c = 0; c < rowValues.Length; c++)
                rowValues[c] = blocks[c].rows[r];

            bars[r].AddBlocks(rowValues, characterData);
            rowValues.Initialize();
        }
    }

    public void BlockCleared(BarBlock block, bool wasCorrect)
    {
        var data = block.characterData;
        block.characterData = null;

        if (wasCorrect)
            data.correctBlocks++;
        else
            data.missedBlocks++;

        var reset = data.CheckComplete();

        if (reset)
            _activeCharacters.Remove(data);
    }

    /// <summary>
    /// Needed so we can check the score and change the colour if
    /// necessary.
    /// </summary>
    /// <returns>The eye room</returns>
    public WildcardRoom GetEyeRoom()
    {
        return _eyeRoom;
    }

    public void SetTextMesh(BlockTextMover mover)
    {
        if (_textMesh) return;
        _textMesh = mover;
    }

    public void PushBarMessage(string message)
    {
        if (string.IsNullOrEmpty(message))
            return;

        _textMesh.ShowMessage(message);
    }

    IEnumerator SendBlockPulse()
    {
        for(var i =0; i < bars.Length; i++)
        {
            bars[i].PulseBlock();
        }

        yield return new WaitForSeconds( _pulseInterval );
        StartCoroutine( SendBlockPulse() );
    }
}
