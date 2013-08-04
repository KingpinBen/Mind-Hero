using UnityEngine;
using System.Collections;

public class InfectionEvent : EventfulObject
{
    public bool rateChange = true;
    public WildcardRoomType targetRoom;
    public float infectionRate;
    public float infectionValue;
    public string triggerText;
    public bool endInfectionOnCleared;

    private HeadScript _headScript;
    private WildcardRoom _targetRoom;

    protected void Awake()
    {
        _headScript = 
            GameObject.FindWithTag("BriansHead").GetComponent<HeadScript>();

        _targetRoom = _headScript.wildcardRooms[(int)targetRoom];
    }

    protected void Start()
    {
        if (!string.IsNullOrEmpty(triggerText))
        {
            var triggerTextTest = GameStrings.GetNodeString("infectionData/messages/" + triggerText);
            
            if (triggerTextTest != "ERROR")
                triggerText = triggerTextTest;
        }
    }

    public override void ToggleObject()
    {
        FireObject();
    }

    public override void FireObject()
    {
        _targetRoom.degenerationRate = infectionRate;

        if (rateChange)
            _targetRoom.roomScoreRaw = infectionValue;

        if (endInfectionOnCleared)
            _targetRoom.TriggerInfectionEvent();

        BarManager.Instance.PushBarMessage(triggerText);
    }
}
