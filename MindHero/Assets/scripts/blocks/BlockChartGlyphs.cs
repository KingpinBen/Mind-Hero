using UnityEngine;
using System.Collections;

public class BlockChartGlyphs : MonoBehaviour
{

    private Material _material;

    void Awake()
    {
        _material = renderer.material;
    }
	
    public void SetScore(int score)
    {
        if (score < 0 || score > 9) return;

        var x = (.2f * score) % 1;
        var y = (score > 4) ? 0 : 0.5f;
        var offset = new Vector2(x, y);

        _material.SetTextureOffset("_MainTex", offset);
    }
}
