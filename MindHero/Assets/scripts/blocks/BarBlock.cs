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

    /// <summary>
    /// The block has passed its mark and needs resetting
    /// </summary>
    /// <param name="bar">The bar to which it belongs</param>
    /// <param name="quickFinish">Should the block disappear now or wait</param>
    public void FinishBlock(RoomBar bar, bool quickFinish)
    {
        if (!_bar)
            _bar = bar;

        if (quickFinish)
            DeactivateBlock();
        else
            StartCoroutine(DeactivateBlockCountdown());
    }

    /// <summary>
    /// Used to reset the block to be used again.
    /// </summary>
    private void DeactivateBlock()
    {
        if (_backupColor != Color.black)
            renderer.material.color = _backupColor;

        gameObject.SetActive(false);
        transform.localPosition = Vector3.zero;
        _bar.QueueVirtualBlock(this);
    }

    /// <summary>
    /// Delay the time before the block gets reset.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeactivateBlockCountdown()
    {
        yield return new WaitForSeconds(1f);

        DeactivateBlock();
    }

    public CharacterData characterData { get; set; }
}
