using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BankRoom : Room
{
    public GameObject workerPrefab;
    public GameObject particlePrefab;
    public GUISkin guiSkin;
    public Color[] workerColours;
    public int initialWorkerCount = 10;
    public Transform spawnLocation;
    public float respawnTimer = 2.0f;

    private readonly Queue<Worker> _inactiveWorkers = new Queue<Worker>();  //  Used up, require recharging
    private readonly List<Worker> _bankedWorkers = new List<Worker>();    //  Available for placement
    private HeadScript _headScript;
    private Camera _camera;
    private float _elapsed;
    private WildcardRoom _noseRoom;

    private Matrix4x4 _guiMatrix;
    private Rect _guiRect;

	protected override void Start ()
	{
	    _headScript = GameObject.FindWithTag("BriansHead").GetComponent<HeadScript>();
	    _camera = _headScript.headCamera;
        _noseRoom = _headScript.wildcardRooms[1];

	    base.Start();

        for(var i = 0; i < initialWorkerCount; i++)
        {
            var worker = Instantiate(workerPrefab) as GameObject;
            var script = worker.GetComponent<Worker>();

            script.workerBank = this;  // fix

            script.skinnedMesh.material.color =
                workerColours[Random.Range(0, workerColours.Length)];

            _allWorkers.Add(new WorkerInfo(script, 0));
            _bankedWorkers.Add(script); 
        }
	}
	
	void Update () {
	    if (_inactiveWorkers.Count > 0)
	    {
	        if (_elapsed >= respawnTimer * (1 + _noseRoom.GetScoreNegative()))
	        {
	            var worker = _inactiveWorkers.Dequeue();

                worker.transform.position = spawnLocation.position;
                worker.gameObject.SetActive(true);
                worker.GiveTask(null);
	            _elapsed = 0.0f;
	        }
	        else
	            _elapsed += Time.deltaTime;
	    }

        UpdateGUI();
	}

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Worker") return;

        var worker = other.GetComponent<Worker>();
        var info = GetWorkerInfo(worker);
        
        //  We change the parent here as it won't be forced from
        //  GetWorkerInfo as all workers in scene should be 
        //  in _allWorkers, so it will never get past the forloop
        worker.transform.parent = transform;

        _bankedWorkers.Add(worker);
        
        DistributeTask(info);
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.tag != "Worker") return;

        var worker = other.GetComponent<Worker>();
        _bankedWorkers.Remove(worker);

        worker.transform.parent = null;
    }

    public void DeactivateWorker(Worker worker)
    {
        var prefab = Instantiate(particlePrefab) as GameObject;
        var position = worker.transform.position;

        position.z = 1.0f;
        position.y += 1.0f;

        prefab.transform.position = position;

        var task = worker.GetTask();
        if (task) 
            task.SetInUse(false);

        worker.gameObject.SetActive(false);

        _inactiveWorkers.Enqueue(worker);
    }

    void ActivateWorker(Vector3 position, bool remove)
    {
        if (_bankedWorkers.Count == 0) return;

        //  Remove the one on top and activate it.
        var top = _bankedWorkers.Count - 1;
        var worker = _bankedWorkers[top];

        if (remove) 
            _bankedWorkers.RemoveAt(top);

        worker.transform.position = position;
        worker.gameObject.SetActive(true);
    }

    public void ActivateWorker(Vector3 position)
    {
        ActivateWorker(position, true);
    }

    void UpdateGUI()
    {
        _guiRect = new Rect(0, 0, 120, 50);
        var offset = new Vector3(_camera.pixelRect.x * 1.02f,
                                (_camera.pixelRect.y + _camera.pixelHeight) * .955f, 0f);
        var scale = Screen.height * .001f;

        _guiMatrix = Matrix4x4.TRS(offset, Quaternion.identity,
                                   new Vector3(scale, scale, 1.0f));
    }

    private void OnGUI()
    {
        GUI.matrix = _guiMatrix;   
        GUI.skin = guiSkin;
        GUI.Box(_guiRect, "Sleeping\nBlobs");
        GUI.Label(_guiRect, _bankedWorkers.Count.ToString());
    }
}
