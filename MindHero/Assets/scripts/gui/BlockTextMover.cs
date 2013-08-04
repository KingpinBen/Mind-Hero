using UnityEngine;
using System.Collections;

public class BlockTextMover : MonoBehaviour
{
    private bool _moving;
    private bool _visible;
    private TextMesh _textMesh;
    private float _targetHeight;
    
	void Start ()
	{
	    _textMesh = GetComponent<TextMesh>();
        BarManager.Instance.SetTextMesh(this);
	}
	
	void Update () {
	    if (_moving)
	    {
	        var pos = transform.localPosition;
            var delta = Time.deltaTime * ((_visible) ? .5f : -.5f);

	        pos.y += delta;
	        
            if (_visible)
            {
                if (pos.y > _targetHeight)
                {
                    _moving = false;
                    pos.y = _targetHeight;
                    StartCoroutine(StartHiding());
                }
            }
            else
            {
                if (pos.y < _targetHeight)
                {
                    _moving = false;
                    pos.y = _targetHeight;
                }
            }

            transform.localPosition = pos;
	    }
	}

    public void Hide()
    {
        _targetHeight = -.65f;
        _moving = true;
        _visible = false;
    }

    public void ShowMessage(string text)
    {
        //  If it's already displaying a message and waiting to remove it,
        //  we should change the text then restart the reset.
        if (_visible && !_moving)
        {
            StopAllCoroutines();
            StartCoroutine(StartHiding());
        }

        _textMesh.text = text;

        _targetHeight = -.475f;
        _moving = true;
        _visible = true;
    }

    private IEnumerator StartHiding()
    {
        yield return new WaitForSeconds(3.0f);

        Hide();
    }
}
