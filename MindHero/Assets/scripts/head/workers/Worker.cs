using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof (CharacterController))]
public class Worker : GrabObject
{
    public enum WorkerStatus
    {
        Idle,
        Busy
    }

    public SkinnedMeshRenderer skinnedMesh;

    public float movementSpeed = 1.0f;
    public BankRoom workerBank;

    private bool _targetReached;
    private Animator _animator;
    private CharacterController _controller;
    private float _elapsed;
    private float _verticalMovement;
    private float _horizontalMovement;
    private WorkerTask _task;
    private WorkerStatus _status = WorkerStatus.Idle;

    private const float GRAVITY = 15.0f;

    protected override void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    protected override void Update()
    {
        var grounded = _controller.isGrounded;

        if (_grabbed == false)
        {
            switch (grounded)
            {
                case true:
                    if (_targetReached == false)
                    {
                        if (_task)
                        {
                            var distanceToTask = 
                                Math.Abs(_task.transform.position.x - transform.position.x);

                            if (distanceToTask > 0.05f)
                            {
                                var movement = new Vector3(_horizontalMovement, _verticalMovement, 0);
                                movement *= movementSpeed * Time.deltaTime;
                                _controller.Move(movement);
                            }
                            else
                            {
                                var taskPos = transform.position;
                                taskPos.x = _task.transform.position.x;
                                transform.position = taskPos;

                                _task.SetInUse(true);
                                StopMoving();
                            }
                        }
                        else
                        {
                            _controller.Move(Vector3.zero);
                            StopMoving();
                        }
                    }
                    break;
                case false:
                    var position = transform.position;

                    if (position.z > 0.5f)
                    {
                        position.z -= Time.deltaTime*5f;
                        if (position.z < 0.5f) 
                            position.z = 0;
                        transform.position = position;
                    } 
                    else
                    {
                        _verticalMovement = 
                            Mathf.Clamp(_verticalMovement - (GRAVITY * Time.deltaTime), -10, 5);
                        _controller.Move(new Vector3(0, _verticalMovement, 0)*Time.deltaTime);
                    }
                    break;
            }
        }
    }

    private void GoToTarget()
    {
		_horizontalMovement = 0;
		var dis = Math.Abs(_task.transform.position.x - transform.position.x);
		
		if (dis > 0.2)
		{
			if (_task.transform.position.x > transform.position.x)
	            _horizontalMovement = Mathf.Abs(movementSpeed);
	        else if (_task.transform.position.x < transform.position.x)
	            _horizontalMovement = -Mathf.Abs(movementSpeed);
	
	        if (_horizontalMovement > 0)
	        {
	            transform.rotation = Quaternion.Euler(0, 90, 0);
	        } 
	        else
	        {
	            transform.rotation = Quaternion.Euler(0, -90, 0);
	        }
	
	        _animator.SetBool("MovingToTarget", true);
		}
		else
		{
			var pos = transform.position;
			pos.x = _task.transform.position.x;
			transform.position = pos;
		}
    }

    public void GiveTask(WorkerTask task)
    {
        _targetReached = false;

        if (_task)
            _task.SetInUse(false);


        if (task)
        {
            _task = task;
            GoToTarget();
        }
        else
        {
            _task = null;
            _status = WorkerStatus.Idle;
        }
    }

    public WorkerTask GetTask()
    {
        return _task;
    }

    public override void PickUp()
    {
        base.PickUp();

        _animator.SetBool("Grabbed", true);
        _controller.Move(Vector3.zero);
        transform.rotation = Quaternion.identity;

        GiveTask(null);
        _verticalMovement = 0.0f;
    }

    public override void Drop()
    {
        base.Drop(); // drop da.. base? OHYEEAH

        _animator.SetBool("Grabbed", false);
        _verticalMovement = 0.0f;
    }

    public void SetStatus(WorkerStatus status)
    {
        _status = status;
    }

    public WorkerStatus GetStatus()
    {
        return _status;
    }

    private void OnDrawGizmos()
    {
        if (_task != null)
            Gizmos.DrawLine(transform.position, _task.transform.position);
    }

    public void DeactivateWorker()
    {
        workerBank.DeactivateWorker(this);
    }

    void StopMoving()
    {
        _targetReached = true;
        _horizontalMovement = 0.0f;
        transform.rotation = Quaternion.identity;
        _animator.SetBool("MovingToTarget", false);
    }
}
