using System;
using UnityEngine;

public class BarManager : MonoBehaviour
{
    public static BarManager Instance;

    public RoomBar[] bars = new RoomBar[7];
    public float blockSpeed = 1.0f;

    private float _elapsed;
    private float _pulseInterval;
    private CharacterData _activeCharacterData;
    private WildcardRoom _eyeRoom;
    private BlockTextMover _textMesh;

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
    }

    private void Update()
    {
        if (_elapsed >= _pulseInterval)
        {
            for (var i = 0; i < bars.Length; i++)
                bars[i].PulseBlock();

            _elapsed = 0.0f;
        }

        _elapsed += Time.deltaTime;
    }

    public void ReceivePush(BlockColumnArray[] blocks, CharacterData characterData)
    {
        if (blocks == null) throw new ArgumentNullException("blocks");
        if (characterData == null) throw new ArgumentNullException("characterData");
        if (_activeCharacterData != null) return;

        _activeCharacterData = characterData;

        //  Size of the amount of columns
        var rowValues = new bool[blocks.Length];

        for (var r = 0; r < bars.Length; r++)
        {
            for (var c = 0; c < rowValues.Length; c++)
                rowValues[c] = blocks[c].rows[r];

            bars[r].AddBlocks(rowValues);
            rowValues.Initialize();
        }
    }

    public void BlockCleared(bool wasCorrect)
    {
        if (wasCorrect)
            _activeCharacterData.correctBlocks++;
        else
            _activeCharacterData.missedBlocks++;

        var reset = _activeCharacterData.CheckComplete();

        if (reset)
            _activeCharacterData = null;
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
}
