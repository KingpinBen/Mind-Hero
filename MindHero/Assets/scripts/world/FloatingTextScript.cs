using UnityEngine;
using System.Collections;

public class FloatingTextScript : MonoBehaviour
{
    public string textToDisplay;
    public float movementSpeed = 1.0f;
    public float timeToLive = 2.0f;

    private TextMesh _textMesh;

    void Start()
    {
        _textMesh = GetComponent<TextMesh>();
        _textMesh.text = textToDisplay;
        StartCoroutine(KillObject());
    }
	
	void Update ()
	{
	    var pos = transform.position;
	    pos.y += Time.deltaTime*movementSpeed;
	    transform.position = pos;
	}

    IEnumerator KillObject()
    {
        yield return new WaitForSeconds(timeToLive);
        Destroy(gameObject);
    }
}
