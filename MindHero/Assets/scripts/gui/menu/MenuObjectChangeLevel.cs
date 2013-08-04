using UnityEngine;
using System.Collections;

public class MenuObjectChangeLevel : MenuObjectSelectable
{
    public string levelName;
    public LevelType levelType;
    public LevelDifficulty speed = LevelDifficulty.Walk;

    private LevelMenuScreenData _data;

    protected override void Awake()
    {
        var lType = levelType.ToString().ToLower();

        _data.sceneName = levelName;
        _data.title = GameStrings.GetNodeString("menuScreenLevelData/" + lType
                + "/speed" + ((int)speed).ToString() + "/title");

        _data.description = GameStrings.GetNodeString("menuScreenLevelData/" + lType
                + "/speed" + ((int)speed).ToString() + "/desc");
    }

    protected override void Update()
    {
        base.Update();

        if (_mouseOvered)
            if (Input.GetMouseButtonDown(0))
                _handler.gui.NewMenuObjectSelected(this);
    }

    protected override void ActivateMenuObject(MenuObjectsHandler toOpen)
    {
        //  Overriding so we don't do anything on this call.
    }

    public LevelMenuScreenData GetLevelData()
    {
        return _data;
    }
}
