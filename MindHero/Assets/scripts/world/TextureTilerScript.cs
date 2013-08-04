using UnityEngine;
using System.Collections;

public class TextureTilerScript : MonoBehaviour {
	
	public float timesToTile = 1;
	
	private Material _material;
	
	void Awake() {
		_material = renderer.material;
	}

	void Start () {
		
		//	Change the scale to be the correct (best) aspectratio
		//	for the texture in relation to the y scale to stop 
		//	scaling artifacts.
		var texture = _material.mainTexture;
		var textureAspectRatio = texture.width / texture.height;
		var scale = transform.localScale;
		
		scale.x = ((scale.y * textureAspectRatio) * timesToTile ) * .5f;
		transform.localScale = scale;
		
		_material.SetTextureScale("_MainTex", new Vector2(timesToTile, 1));
	}
	
	void Update () {
	
	}
}
