using UnityEngine;
using System.Collections;

public class ScrollingHeadBackground : MonoBehaviour
{

    public float scrollingSpeed;

    private Material _material;
    private Vector2 _offset;

	void Start ()
	{
	    _material = renderer.material;
	}
	
	void Update ()
	{
	    _offset.x = (_offset.x + (Time.deltaTime*-scrollingSpeed))%1;

	    _material.SetTextureOffset("_MainTex", _offset);
	}
}
