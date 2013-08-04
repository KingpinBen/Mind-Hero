using UnityEngine;
using System.Collections;
using System.Xml;
using System;

public class MenuDifficultyGui : MonoBehaviour
{

    public GUISkin skin;

    private Matrix4x4 _matrix;
    private Rect _guiRect;
    private Rect _backRect;
    private LevelMenuScreenData _levelData;
    private MenuObjectsHandler _handler;
    private Material _currentlySelectedLevel;

    public bool beingShown { get; set; }

    private void Awake()
    {
        _handler = GetComponent<MenuObjectsHandler>();
    }

	void Update () 
    {
	    UpdateGui();
	}

    void UpdateGui()
    {
        var scale = Screen.height * 0.001f;
        _guiRect = new Rect(0,0, 500, 750);
        

        var offset = new Vector3((Screen.width - ((_guiRect.width + 150) * scale)),
                                    (Screen.height * .5f) - ((_guiRect.yMax * .5f) * scale), 0);

        _backRect = new Rect(-1024, 0, 50, _guiRect.yMax);
        
        _matrix = Matrix4x4.TRS(offset, Quaternion.identity, new Vector3(scale, scale, 1));
    }

    void OnGUI()
    {
        if (beingShown == false)    
            return;

        GUI.skin = skin;
        GUI.matrix = _matrix;

        //  ******************************
        //  Do the back button.
        //  ******************************
        GUILayout.BeginArea(_backRect);
        if (GUILayout.Button("", skin.customStyles[3]))
            _handler.GoBack();
        GUILayout.EndArea();


        //  ***************************************************************
        //  We'll exit out if we don't have any data loaded as nothing below 
        //  should work.
        //  ***************************************************************
        if (string.IsNullOrEmpty(_levelData.sceneName))
            return;

        //  ******************************
        //  Title and Description.
        //  ******************************
        GUILayout.BeginArea(_guiRect, skin.customStyles[0]);
        GUILayout.Label(_levelData.title, skin.customStyles[1]);
        GUILayout.FlexibleSpace();
        GUILayout.Label(_levelData.description);
        GUILayout.FlexibleSpace();

        GUILayout.BeginVertical();

        //  TODO: Need to store this data somewhere.
        //  ******************************
        //  Highscore and Play button
        //  ******************************
        GUILayout.BeginHorizontal(skin.customStyles[2]);
        GUILayout.Label("Previous Best Follower Total", skin.customStyles[4]);
        GUILayout.Label("0", skin.customStyles[4]);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Play")) 
            Application.LoadLevel(_levelData.sceneName);

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    public void NewMenuObjectSelected(MenuObjectChangeLevel changeLevel)
    {
        _levelData = changeLevel.GetLevelData();

        if (_currentlySelectedLevel)
            _currentlySelectedLevel.color = new Color(.5f, .5f, .5f);

        _currentlySelectedLevel = changeLevel.renderer.material;
        _currentlySelectedLevel.color = Color.white;
    }
}
