using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof (BoxCollider))]
public class RoomBar : MonoBehaviour
{

    public GameObject blockPrefab;
    public Room room;
    public Material blockMaterial;
    public Color blockColor;
    public BlockChartGlyphs counterGlyphs;
    public GameObject particlePrefab;
    public BarManager manager { get; set; }

    private BarBlock _blockScript;

    //  Holds references to all the blocks for the bar.
    private readonly List<BarBlock> _blockList = new List<BarBlock>();
    //  The blocks which aren't currently being used.
    private readonly Queue<BarBlock> _blockQueue = new Queue<BarBlock>();
    //  The reserved blocks for adding to the game.
    private readonly Queue<BarBlock> _blocksToAdd = new Queue<BarBlock>();

    private readonly List<BlockParticlePop> _particleSystems = new List<BlockParticlePop>();
    private readonly Queue<BlockParticlePop> _queuedParticles = new Queue<BlockParticlePop>();
    private readonly List<BlockParticlePop> _activeParticles = new List<BlockParticlePop>(); 

    private Material _mat;

    private void Awake()
    {
        _blockScript = blockPrefab.GetComponent<BarBlock>();
        _mat = new Material(blockMaterial);
    }

    private void Start()
    {
        int i;

        _mat.color = blockColor;

        /* Just give it 4 blocks to start with. If more are needed
        * to be shown at once, they'll be created on AddBlocks but it may cause
        * slowdown */
        QueueUpNewBarBlocks(4);

        for (i = 0; i < 3; i++)
        {
            var pe = Instantiate(particlePrefab) as GameObject;
            pe.transform.parent = transform;
            pe.transform.localPosition = new Vector3(-14.3f + .5f, .2f, 0);

            _particleSystems.Add(pe.GetComponent<BlockParticlePop>());
            _particleSystems[i].particleSystem.renderer.material = _mat;
            _queuedParticles.Enqueue(_particleSystems[i]);
        }

        room.OnWorkerChange += ChangeWorkerCountText;
    }

    private void Update()
    {
        var i = 0;

        for (; i < _activeParticles.Count; i++)
        {
            var ps = _activeParticles[i];
            if (ps.particleSystem.IsAlive()) continue;

            ps.particleSystem.Stop();
            _queuedParticles.Enqueue(ps);
        }
    }

    public void AddBlocks(bool[] blocks)
    {
        var eyeInBadCondition = manager.GetEyeRoom().GetScoreNegative();

        for (var i = 0; i < blocks.Length; i++)
        {
            if (blocks[i])
            {
                /* 
                 * Check if we have enough spare blocks in the queue
                 * otherwise we'll need to make some more.
                 */
                if (_blockQueue.Count == 0)
                    QueueUpNewBarBlocks(1);

                var block = _blockQueue.Dequeue();

                //  Chance to be bad
                if (-eyeInBadCondition > Random.Range(0.0f, 1.0f))
                    block.newColor = Color.white;

                _blocksToAdd.Enqueue(block);
                
            }
            else
            {
                //  We add null here so that we can use it as a placeholder for 
                //  an empty block in the column.
                _blocksToAdd.Enqueue(null);
            }
        }
    }

    public void RemoveBlock(BarBlock block)
    {
        var count = room.GetWorkerCount();

        if (count > 0)
        {
            block.gameObject.SetActive(false);
            block.transform.localPosition = Vector3.zero;
            manager.BlockCleared(room.RemoveWorker());

            if (particlePrefab) 
                SpawnParticleEmitter();

            _blockQueue.Enqueue(block);
        } 
        else
        {
            block.FinishBlock(this);
            manager.BlockCleared(false);
        }
    }

    public void PulseBlock()
    {
        if (_blocksToAdd.Count > 0)
        {
            var obj = _blocksToAdd.Dequeue();

            if (obj)
                obj.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider body)
    {
        var block = body.GetComponent<BarBlock>();

        if (block)
            RemoveBlock(block);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue*0.5f;
        var size = ((BoxCollider) collider).center.x;
        Gizmos.DrawCube(transform.position + ((BoxCollider) collider).center*.5f, new Vector3(size, 1, 1));
    }

    public void ChangeBlockSpeed(float speed)
    {
        _blockScript.movementSpeed = speed;
        for (var i = 0; i < _blockList.Count; i++)
            _blockList[i].GetComponent<BarBlock>().movementSpeed = speed;
    }

    public void ChangeWorkerCountText()
    {
        var count = room.GetWorkerCount();
        counterGlyphs.SetScore(count);
    }

    public void SpawnParticleEmitter()
    {
        if (_queuedParticles.Count == 0) return;

        var ps = _queuedParticles.Dequeue();
        ps.Pop();
        _activeParticles.Add(ps);
    }

    /// <summary>
    /// Used for when a block doesn't have a place to call home if passing the removal trigger.
    /// </summary>
    public void QueueVirtualBlock(BarBlock block)
    {
        _blockQueue.Enqueue(block);
    }

    void QueueUpNewBarBlocks(uint count)
    {
        for(uint i = 0; i < count; i++)
        {
            var go = Instantiate(blockPrefab) as GameObject;
            var script = go.GetComponent<BarBlock>();

            go.transform.parent = transform;
            go.transform.localPosition = Vector3.zero;

            go.renderer.material = _mat;

            _blockList.Add(script);
            _blockQueue.Enqueue(script);
        }
    }
}
