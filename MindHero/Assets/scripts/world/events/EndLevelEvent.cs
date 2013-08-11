using UnityEngine;
using System.Collections;

public class EndLevelEvent : EventfulObject
{
    public string nextLevelName;
    public float delayBeforeFire = 0.0f;
    public GUISkin scoreSkin;
    public LevelDifficulty difficulty = LevelDifficulty.Walk;
    public LevelType levelType = LevelType.Egypt;

    private FollowerCrowdScript _followerScript;
    private XmlNode _levelLocalNode;
    private Matrix4x4 _guiMatrix;
    private Texture2D _backgroundTexture;

    private void Awake()
    {
        _followerScript = Camera.mainCamera.GetComponent< FollowerCrowdScript >();
        
        _backgroundTexture = new Texture2D( 1, 1 );
        _backgroundTexture.SetPixel( 1, 1, Color.black );
        _backgroundTexture.Apply();

        var scale = ((Screen.width > Screen.height) ? Screen.height : Screen.width) * 0.001f;
        _guiMatrix = Matrix4x4.TRS(
            new Vector3((Screen.width * .5f) - ((800 * .5f) * scale),   //  760 is the fixed width of the yellow text box
                         (Screen.height * .5f) - ((820 * .5f) * scale), 0), //  820 has no specific meaning but it's high enough to let it stretch down.
            Quaternion.identity, new Vector3(scale, scale, 1));
    }

    private void Start()
    {
        _levelLocalNode = XmlHandler.FindNodeWithExactTagsPath(new[]
                                                                    {
                                                                        "menuScreenLevelData",
                                                                        levelType.ToString().ToLower(),
                                                                        "speed" + ( ( int ) difficulty ).ToString()
                                                                    });
    }

    private IEnumerator ChangeLevel()
    {
        yield return new WaitForSeconds( delayBeforeFire );

        FireObject();
    }

    public override void ToggleObject()
    {
        if ( delayBeforeFire > 0 )
            StartCoroutine( ChangeLevel() );
        else
            FireObject();
    }

    public override void StartObject()
    {
        base.StartObject();

        //  We want to stop all the stuff in the background (for now anyway)
        Time.timeScale = 0.0f;
    }

    public override void FireObject()
    {
        _eventActive = true;
    }

    private void OnGUI()
    {
        if ( _eventActive == false )
            return;

        var scores = _followerScript.GetFollowerScores();

        GUI.color = Color.white * .5f;
        GUI.DrawTexture( new Rect( 0, 0, Screen.width, Screen.height ), _backgroundTexture );

        GUI.skin = scoreSkin;
        GUI.depth = -1;
        GUI.matrix = _guiMatrix;
        GUI.color = Color.white;

        #region Group
        GUILayout.BeginVertical();

        #region Big, Top box
        GUILayout.BeginVertical(scoreSkin.customStyles[0]);

        GUILayout.Label("LEVEL COMPLETE");
        GUILayout.FlexibleSpace();
        GUILayout.Label(_levelLocalNode[3].contents, scoreSkin.customStyles[1]);

        #region Basic Stats
        GUILayout.BeginVertical();

        #region Cured Infections
        GUILayout.BeginHorizontal(); 
        GUILayout.FlexibleSpace();
        GUILayout.Label("INFECTIONS CURED: ", scoreSkin.customStyles[2]);
        GUILayout.Label(" " + scores.infectionsCured, scoreSkin.customStyles[2]);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        #endregion

        #region Followers Lost
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("FOLLOWERS LOST: ", scoreSkin.customStyles[2]);
        GUILayout.Label(" " + scores.failedCharacters, scoreSkin.customStyles[2]);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        #endregion

        #region Followers Missed
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("FOLLOWERS MISSED: ", scoreSkin.customStyles[2]);
        GUILayout.Label(" " + scores.lostCharacters, scoreSkin.customStyles[2]);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        #endregion

        GUILayout.EndVertical();
        #endregion

        #region Final Follower Count
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Final Followers: ", scoreSkin.customStyles[3]);
        GUILayout.Label(" " + scores.successfulCharacters, scoreSkin.customStyles[3]);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        #endregion

        GUILayout.EndVertical();
        #endregion

        #region Buttons
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Replay"))
            Application.LoadLevel(Application.loadedLevel);

        if (!string.IsNullOrEmpty(nextLevelName))
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Continue"))
                Application.LoadLevel(nextLevelName);
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Menu"))
            Application.LoadLevel("mainMenu");
        GUILayout.EndHorizontal();
        #endregion

        GUILayout.EndVertical();
        #endregion
    }

    public Texture2D GetPixelTexture()
    {
        return _backgroundTexture;
    }
}
