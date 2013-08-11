using UnityEngine;
using System.Collections;

public class ScrollingSkybox : MonoBehaviour
{
    public float scrollingSpeed = 1.0f;

    private Material _skyboxMaterial;
    private Vector2 _currentTextureOffset = new Vector2(0, 0);

    void Awake()
    {
        _skyboxMaterial = RenderSettings.skybox;

        _skyboxMaterial.SetTextureScale("_BackTex", new Vector2(camera.pixelWidth / camera.pixelHeight, 1));
        SetOffset();
    }

	void Update ()
	{
	    _currentTextureOffset.x += Time.deltaTime * scrollingSpeed;
        SetOffset();
	}

    void SetOffset()
    {
        _skyboxMaterial.SetTextureOffset("_BackTex", _currentTextureOffset);
    }
}
