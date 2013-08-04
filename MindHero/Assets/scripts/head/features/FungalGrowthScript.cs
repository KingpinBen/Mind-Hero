using UnityEngine;
using System.Collections;

public class FungalGrowthScript : MonoBehaviour
{
    private Material _material;
    private WildcardRoom _wildcardRoom;

	void Start ()
	{
        _wildcardRoom = transform.parent.GetComponent<WildcardRoom>();
	    _material = renderer.material;
	}

	void Update ()
	{
	    _material.SetFloat("_AlphaCut", -_wildcardRoom.GetScoreNegative());
	}
}
