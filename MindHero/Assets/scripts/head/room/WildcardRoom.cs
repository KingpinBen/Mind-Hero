using UnityEngine;
using System.Collections;

public class WildcardRoom : Room
{
    public float degenerationRate = 1f;
    public float regenerationWorkerRate = 2.0f;

    private float _roomScoreModifier;
    private bool _infectionEvent;
    private readonly string[] _warningStrings = new string[5];
    private FollowerCrowdScript _followerCrowdScript;

    /// <summary>
    /// Gets the room score value between -1 and 1.
    /// -1=bad,0=no change, 1=good.
    /// </summary>
    public float roomScoreRaw
    {
        get { return _roomScoreModifier; }
        set { _roomScoreModifier = MathHelper.Clamp(-1, 1, _roomScoreModifier + value); }
    }

    /// <summary>
    /// Gets the room score by value between 0-1
    /// </summary>
    public float roomScore01
    {
        get { return (_roomScoreModifier*0.5f) + .5f; }
    }

    private void Awake()
    {
        _followerCrowdScript = Camera.mainCamera.GetComponent< FollowerCrowdScript >();
    }

    protected override void Start()
    {
        base.Start();

        var lowerRoomName = roomType.ToString().ToLower();
        var xmlNode = XmlHandler.FindTagWithParentTag( new[]
                                                           {
                                                               "infectionData",
                                                               lowerRoomName,
                                                           } );

        _warningStrings[0] = xmlNode[0].contents;   //  0 warning
        _warningStrings[1] = xmlNode[1].contents;   //  25 warning
        _warningStrings[2] = xmlNode[2].contents;   //  50 warning
        _warningStrings[3] = xmlNode[3].contents;   //  75 warning
        _warningStrings[4] = xmlNode[4].contents;   //  100 warning
    }

	protected virtual void Update ()
	{
        if (degenerationRate <= Mathf.Epsilon) 
            return;

	    var workerCount = _allWorkers.Count;
	    var startValue = roomScoreRaw;

        roomScoreRaw = (workerCount > 0) ? 
            Time.deltaTime * ((regenerationWorkerRate * workerCount) * 0.001f):
            roomScoreRaw = -Time.deltaTime * (degenerationRate * 0.001f);

	    if (CheckMajorChange(startValue, 0))
	    {
            BarManager.Instance.PushBarMessage(_warningStrings[0]);

            if (_infectionEvent)
            {
                degenerationRate = 0;
                _infectionEvent = false;
                _followerCrowdScript.InfectionComplete();
                roomScoreRaw = -roomScoreRaw;   //  Zero it.
            }
	    }
	    else
	    {
	        if (CheckMajorChange(startValue, -.25f))
	            BarManager.Instance.PushBarMessage(_warningStrings[1]);
	        else
	        {
	            if (CheckMajorChange(startValue, -.5f))
	                BarManager.Instance.PushBarMessage(_warningStrings[2]);
	            else
	            {
	                if (CheckMajorChange(startValue, -.75f))
	                    BarManager.Instance.PushBarMessage(_warningStrings[3]);
	                else
	                {
	                    if (CheckMajorChange(startValue, -1))
	                        BarManager.Instance.PushBarMessage(_warningStrings[4]);
	                }
	            }
	        }
	    }
	}

    /// <summary>
    /// Gets the score above 0.
    /// </summary>
    /// <returns></returns>
    public float GetScorePositive()
    {
        return (_roomScoreModifier > 0)
                   ? _roomScoreModifier
                   : 0;
    }

    /// <summary>
    /// Gets the room score below 0
    /// </summary>
    /// <returns></returns>
    public float GetScoreNegative()
    {
        return (_roomScoreModifier < 0)
                   ? _roomScoreModifier
                   : 0;
    }

    bool CheckMajorChange(float initialValue, float valueCheck)
    {
        return initialValue < valueCheck && roomScoreRaw >= valueCheck ||
               initialValue > valueCheck && roomScoreRaw <= valueCheck;
    }

    public void TriggerInfectionEvent()
    {
        _infectionEvent = true;
    }
}

public static class MathHelper
{
    /// <summary>
    /// Mathf.Clamp wasn't working correctly above so here's a replacement
    /// </summary>
    /// <param name="lower">min value</param>
    /// <param name="top">max value</param>
    /// <param name="val">value to clamp</param>
    /// <returns></returns>
    public static float Clamp(float lower, float top, float val)
    {
        var min = lower;
        var max = top;

        return val < min ? min : (val > max ? max : val);
    }
}
