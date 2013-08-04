using UnityEngine;
using System.Collections;

public class MenuBlockFader : MonoBehaviour
{
    public float fadeModifier = 0.2f;

    private Material _material;
    private bool _fading;
    private const float ALPHA_MAX_CAP = 0.18f;
    private Color _color;

	void Awake ()
	{
	    _material = renderer.material;
	    _color = _material.color;
	    
	}

    void Start()
    {
        enabled = false;
    }

	void Update () {
	    if (_fading)
	    {
            _color.a += Time.deltaTime * fadeModifier;
            _material.color = _color;

            if (_color.a >= ALPHA_MAX_CAP) enabled = false;
	    } 
        else
	    {
	        //  Just been pinged
	        _color.a -= Time.deltaTime*(fadeModifier*2f);
	        _material.color = _color;

            if (_color.a <= 0) _fading = true;
	    }
	}

    public bool Ping()
    {
        if (enabled) return false;

        enabled = true;
        _fading = false;

        return true;
    }
}
