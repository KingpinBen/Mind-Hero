using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class MenuObjectSelectable : MonoBehaviour
{

    public MenuObjectsHandler openObject;

    protected MenuObjectsHandler _handler;
    protected bool _mouseOvered;
    protected bool _inPosition;
    private Transform _target;
    protected bool _transitioningOut;

    public bool transitioningOut
    {
        get { return _transitioningOut; }
        set { _transitioningOut = value; }
    }
    public MenuObjectsHandler handler
    {
        get { return _handler; }
        set { _handler = value; }
    }
    public Transform targetPosition
    {
        get { return _target; }
        set { _target = value; }
    }

    protected virtual void Awake()
    {
        
    }

	protected virtual void Start() 
    {
	
	}

    protected virtual void Update()
    {
        _inPosition = !((_target.position - transform.position).magnitude > 0.01);

        if (_inPosition && !_transitioningOut)
        {
            if (_mouseOvered)
                if (Input.GetMouseButtonDown(0))
                    ActivateMenuObject(openObject);
        }
        else
        {
            transform.position =
                Vector3.Lerp(transform.position, _target.position, Time.deltaTime*5f);
        }
    }

    protected virtual void ActivateMenuObject(MenuObjectsHandler toOpen)
    {
        _handler.SetState(false, false, null);
        toOpen.SetState(true, true, _handler);

        _mouseOvered = false;
    }

    protected virtual void OnMouseEnter()
    {
        _mouseOvered = true;
    }

    protected virtual void OnMouseExit()
    {
        _mouseOvered = false;
    }
}
