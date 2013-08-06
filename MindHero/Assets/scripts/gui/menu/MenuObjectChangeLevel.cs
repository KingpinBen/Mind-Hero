using UnityEngine;
using System.Collections;

public class MenuObjectChangeLevel : MenuObjectSelectable
{
    public int sceneId;
    public LevelType levelType;
    public LevelDifficulty speed = LevelDifficulty.Walk;

    private LevelMenuScreenData _data;

    protected override void Awake()
    {
        var lType = levelType.ToString().ToLower();

        _data.sceneId = sceneId;

        XmlNode node = XmlHandler.FindTagWithParentTag( new[]
                                                            {
                                                                "menuScreenLevelData",
                                                                lType.ToString(),
                                                                "speed" + ( ( int ) speed ).ToString()
                                                            } );
        _data.title = node[0].contents;
        _data.description = node[1].contents;
    }

    protected override void Update()
    {
        base.Update();

        if ( _mouseOvered )
            if ( Input.GetMouseButtonDown( 0 ) )
                _handler.gui.NewMenuObjectSelected( this );
    }

    protected override void ActivateMenuObject( MenuObjectsHandler toOpen )
    {
        //  Overriding so we don't do anything on this call.
    }

    public LevelMenuScreenData GetLevelData()
    {
        return _data;
    }
}
