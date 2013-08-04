using UnityEngine;
using System.Collections;

public class ThroatVomScript : MonoBehaviour
{
    private Material _material;
    private Vector2 _offset;
    private WildcardRoom _mouthRoom;
    private float _lastMouthScore;

    private void Awake()
    {
        var head = GameObject.FindWithTag("BriansHead").GetComponent<HeadScript>();
        _mouthRoom = head.wildcardRooms[2];
    }

    private void Start()
    {
        _material = renderer.material;
        _offset = Vector2.zero;
    }

    private void Update()
    {
        var score = 1 - _mouthRoom.roomScore01;
        if (score != _lastMouthScore)
        {
            _lastMouthScore = score;
            _material.SetFloat("_AlphaCut", score);
        }


        _offset.x += Time.deltaTime*0.1f;
        _material.SetTextureOffset("_MaskTex", _offset);
    }
}
