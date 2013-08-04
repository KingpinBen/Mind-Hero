using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class HeadScript : MonoBehaviour
{

    public Room[] rooms;
    public WildcardRoom[] wildcardRooms;
    public HeadJawScript jaw;
    public EyeBehaviourScript eye;
    public BankRoom blobBank;

    public Camera headCamera { get; internal set; }
    private GrabObject _grabbedObject;
    private int _workerMask;
    private int _roomMask;
    private GameObject _mouseTracker;
    private Vector3 _oldPositionOfPickup;
    private bool _holdingObject;
    private bool _canGrab = true;

    private void Awake()
    {
        _mouseTracker = new GameObject();

        headCamera = GameObject.FindWithTag("HeadCamera").camera;

        _workerMask = (1 << 8);
        _roomMask = (1 << 12);

        _mouseTracker.transform.parent = transform;
        _mouseTracker.name = "MouseTracker";
    }

    private void Start()
    {
        _mouseTracker.transform.position = headCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Update()
    {
        switch (_holdingObject)
        {
            case true:
                var currentMousePosition = headCamera.ScreenToWorldPoint(Input.mousePosition);
                currentMousePosition.z = 2.0f;
                _mouseTracker.transform.position = currentMousePosition;
                
                if (Input.GetMouseButtonUp(1))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(currentMousePosition, Vector3.back, out hit, 5, _roomMask))
                    {
                        var room = hit.point;
                        room.y = hit.transform.position.y - 1.2f;
                        room.z = 0;

                        _grabbedObject.transform.position = room;
                    }
                    else
                        _grabbedObject.transform.position = _oldPositionOfPickup;

                    _grabbedObject.Drop();
                    _grabbedObject = null;

                    _holdingObject = false;
                    _canGrab = true;
                }
                else
                {
                    _grabbedObject.transform.position =
                    Vector3.Slerp(_grabbedObject.transform.position,
                                  _mouseTracker.transform.position +
                                  new Vector3(0, -1.2f, 0), Time.deltaTime * 15.0f);  
                }

                break;

            case false:
                

                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit rayHit;
                    var hit = Physics.Raycast(headCamera.ScreenPointToRay(Input.mousePosition), out rayHit, 50.0f,
                                               _roomMask);

                    if (hit && rayHit.collider.tag == "Room")
                    {
                        var position = rayHit.collider.transform.position;
                        position.z = 1;
                        position.y -= 1;

                        blobBank.ActivateWorker(position);
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    if (_canGrab == false) break;

                    RaycastHit rayHit;
                    var hit = Physics.Raycast(headCamera.ScreenPointToRay(Input.mousePosition), out rayHit, 50.0f,
                                               _workerMask);

                    if (hit && rayHit.collider.tag == "Worker")
                    {
                        var worker = rayHit.transform.GetComponent<Worker>();

                        _oldPositionOfPickup = worker.transform.position;

                        worker.PickUp();

                        _grabbedObject = worker;
                        _holdingObject = true;
                    }
                }
                break;
        }
    }

    public Room GetRoom(RoomType roomType)
    {
        for(var i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].roomType == roomType)
                return rooms[i];
        }

        Debug.LogWarning("Could not find room as type '" + roomType.ToString() + "'.");
        return null;
    }
}
