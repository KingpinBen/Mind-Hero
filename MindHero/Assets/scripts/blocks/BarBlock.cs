using UnityEngine;
using System.Collections;

public class BarBlock : MonoBehaviour
{

    public float movementSpeed = 1.0f;

    private RoomBar _bar;
    /// <summary>
    /// Setting it as black as it should never be black. Ever.
    ///  Need to set for a value for comparing later.
    /// </summary>
    private Color _backupColor = Color.black;   

    public Color newColor
    {
        get { return _backupColor; }
        set
        {
            _backupColor = renderer.material.color;
            renderer.material.color = value;
        }
    }

    private void Start()
    {
        if (movementSpeed < 0)
            movementSpeed = Mathf.Abs(movementSpeed);
    }

    private void Update()
    {
        var newPosition = transform.localPosition;
        newPosition.x -= Time.deltaTime*movementSpeed;

        transform.localPosition = newPosition;
    }

    public void FinishBlock(RoomBar bar)
    {
        StartCoroutine(DeactivateBlock());

        if (!_bar) 
            _bar = bar;

        if (_backupColor != Color.black) 
            renderer.material.color = _backupColor;
    }

    private IEnumerator DeactivateBlock()
    {
        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
        transform.localPosition = Vector3.zero;
        _bar.QueueVirtualBlock(this);
    }
}
