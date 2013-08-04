using System.Globalization;
using UnityEngine;
using System.Collections.Generic;

public class WorkerManager : MonoBehaviour
{

    public GameObject workerPrefab;
    public int startWorkerCount = 10;
    public float respawnDelay = 3.0f;
    public Color[] colorOptions;
    public GameObject removerPrefab;
    public GUISkin guiSkin;
    public AudioClip addWorkerAudioClip;

    private readonly List<Worker> _workers = new List<Worker>();
    private readonly Queue<Worker> _inactiveWorkers = new Queue<Worker>();
    private HeadScript _roomManager;
    private float _elapsed;
    private Camera _camera;

    private void Awake()
    {
        GameObject workerObj;

        _roomManager = GetComponent<HeadScript>();
        _camera = _roomManager.headCamera;

        for (var i = 0; i < startWorkerCount; i++)
        {
            workerObj = Instantiate(workerPrefab) as GameObject;

            var workerScript = workerObj.GetComponent<Worker>();
            //workerScript.manager = this;

            workerScript.skinnedMesh.material.color = colorOptions[Random.Range(0, colorOptions.Length)];

            _workers.Add(workerScript);
            _inactiveWorkers.Enqueue(_workers[i]);
        }
    }

    private void Update()
    {
        //  TODO Check if there should be a CD on how long a freshly removed Blob can be reused.
    }

    public void DeactivateWorker(Worker worker)
    {
        var prefab = Instantiate(removerPrefab) as GameObject;
        var position = worker.transform.position;

        position.z = 1.0f;
        position.y += 1.0f;

        prefab.transform.position = position;

        worker.GiveTask(null);
        worker.transform.position = Vector3.zero; //  Temp position just to hide offscreen.
        worker.gameObject.SetActive(false);

        _inactiveWorkers.Enqueue(worker);
    }

    public void ActivateWorker(Vector3 position)
    {
        if (_inactiveWorkers.Count == 0) return;

        //  Remove the one on top and activate it.
        var worker = _inactiveWorkers.Dequeue();

        worker.transform.position = position;
        worker.gameObject.SetActive(true);
    }

    private void OnGUI()
    {
        var rect = new Rect(0, 0, 120, 50);
        var offset = new Vector3(_camera.pixelRect.x * 1.02f, 
                                (_camera.pixelRect.y + _camera.pixelHeight) * .955f, 0f);
        var scale = Screen.height*.001f;
		
        GUI.matrix = Matrix4x4.TRS(offset, Quaternion.identity,
                                   new Vector3(scale, scale, 1.0f));
        GUI.skin = guiSkin;
        GUI.Box(rect, "Sleeping\nBlobs");
        GUI.Label(rect, _inactiveWorkers.Count.ToString());
    }
}
