using UnityEngine;
using System.Collections;

public class EndLevelEvent : EventfulObject
{
    public string nextLevelName;
    public float delayBeforeFire = 0.0f;
    public GUISkin scoreSkin;

    private FollowerCrowdScript _followerScript;

	void Start ()
	{
	    _followerScript = GameObject.FindWithTag("MainCamera").GetComponent<FollowerCrowdScript>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator ChangeLevel()
    {
        yield return new WaitForSeconds(delayBeforeFire);
        FireObject();
    }

    public override void ToggleObject()
    {
        if (delayBeforeFire > 0)
            StartCoroutine(ChangeLevel());
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

    void OnGUI()
    {
        if (_eventActive == false) return;

        var scores = _followerScript.GetFollowerScores();

        //  TODO: Need the matrix

        GUI.skin = scoreSkin;
        GUI.depth = -1;

        GUILayout.BeginArea(new Rect(0f,0f,500f, 300f), scoreSkin.customStyles[0]);
        GUILayout.Label("Score");

        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical(scoreSkin.customStyles[1]);
        GUILayout.Label("Followers Gained: " + scores.successfulCharacters, scoreSkin.customStyles[1]);
        GUILayout.Label("Followers Missed: " + scores.failedCharacters, scoreSkin.customStyles[1]);
        GUILayout.EndVertical();

        GUILayout.BeginVertical(scoreSkin.customStyles[1]);
        GUILayout.Label("Followers Gained: " + scores.successfulCharacters, scoreSkin.customStyles[1]);
        GUILayout.Label("Followers Missed: " + scores.failedCharacters, scoreSkin.customStyles[1]);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Restart Level", scoreSkin.customStyles[1])) 
            Application.LoadLevel(Application.loadedLevelName);

        /*  
         * TODO: Find out the actual passing conditions for the levels
         * as the player shouldn't be able to continue if it fails 
         */
        if (scores.successfulCharacters - scores.lostCharacters > scores.failedCharacters)
        {
            if (GUILayout.Button("Continue..", scoreSkin.customStyles[1]))
                Application.LoadLevel(nextLevelName);
        }
        
        if (GUILayout.Button("Back to Menu", scoreSkin.customStyles[1]))
            Application.LoadLevel(0);

        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }
}
