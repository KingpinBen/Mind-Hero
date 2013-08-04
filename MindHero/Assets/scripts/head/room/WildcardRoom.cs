using UnityEngine;
using System.Collections;

public class WildcardRoom : Room
{
    public float degenerationRate = 1f;
    public float regenerationWorkerRate = 2.0f;

    private float _roomScoreModifier;
    private bool _infectionEvent;
    private readonly string[] _warningStrings = new string[5];

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

    protected override void Start()
    {
        base.Start();

        var lowerRoomName = roomType.ToString().ToLower();
        _warningStrings[0] = 
            GameStrings.GetNodeString("infectionData/" + lowerRoomName + "/warning_0");
        _warningStrings[1] =
            GameStrings.GetNodeString("infectionData/" + lowerRoomName + "/warning_25");
        _warningStrings[2] =
            GameStrings.GetNodeString("infectionData/" + lowerRoomName + "/warning_50");
        _warningStrings[3] =
            GameStrings.GetNodeString("infectionData/" + lowerRoomName + "/warning_75");
        _warningStrings[4] =
            GameStrings.GetNodeString("infectionData/" + lowerRoomName + "/warning_100");
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
