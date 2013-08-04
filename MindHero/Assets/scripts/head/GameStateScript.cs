using UnityEngine;
using System.Collections;

public class GameStateScript : MonoBehaviour
{
    public GUISkin skin;

    private bool _paused;
    private Matrix4x4 _guiMatrix;

    void Start()
    {
        var scale = Screen.height*0.003f;

        var screenOffset = new Vector3((Screen.width * .5f) - ((250f * .5f) * scale),
                                    (Screen.height * .5f) - ((220f * .5f) * scale), 0);

        _guiMatrix = 
            Matrix4x4.TRS(screenOffset, Quaternion.identity, new Vector3(scale, scale, 1));
    }

	void Update () {
	    if (Input.GetKeyDown(KeyCode.P) || 
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.Escape))
	    {
	        ToggleState();
	    }
	}

    void ToggleState()
    {
        _paused = !_paused;

        Time.timeScale = _paused ? 0.0f : 1.0f;
    }

    void OnGUI()
    {
        if (!_paused) return;

        GUI.skin = skin;
        GUI.matrix = _guiMatrix;
        //  Need to make sure this is on top so the other screen borders
        //  don't cause issues.
        GUI.depth = 0;

        GUILayout.BeginArea(new Rect(0f, 0f, 250f, 220f), skin.customStyles[0]);
        GUILayout.Label("Paused");
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Continue"))
            ToggleState();

        //  We need to remember to switch the game back to normal game
        //  speed before we change the scene.

        if (GUILayout.Button("Restart"))
        {
            ToggleState();
            Application.LoadLevel(Application.loadedLevel);
        }
            

        if (GUILayout.Button("Back to Menu"))
        {
            ToggleState();
            Application.LoadLevel("MainMenu");
        }
            
        GUILayout.FlexibleSpace();
        GUILayout.EndArea();
    }
}
