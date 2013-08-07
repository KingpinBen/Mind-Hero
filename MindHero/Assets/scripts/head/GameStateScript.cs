using UnityEngine;
using System.Collections;

public class GameStateScript : MonoBehaviour
{
    public GUISkin skin;

    private bool _paused;
    private Matrix4x4 _guiMatrix;
    private EndLevelEvent _endLevel;
    private Texture2D _texture;

    private void Start()
    {
        var scale = ( ( Screen.width > Screen.height ) ? Screen.height : Screen.width ) * 0.001f;
        var screenOffset = new Vector3( ( Screen.width * .5f ) - ( ( 520f * .5f ) * scale ),
                                        ( Screen.height * .5f ) - ( ( 220f * .5f ) * scale ), 0 );

        _guiMatrix =
            Matrix4x4.TRS( screenOffset, Quaternion.identity, new Vector3( scale, scale, 1 ) );

        _endLevel = GetComponent< EndLevelEvent >();
        _texture = _endLevel.GetPixelTexture();
    }

    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.P ) ||
             Input.GetKeyDown( KeyCode.Space ) ||
             Input.GetKeyDown( KeyCode.Escape ) )
        {
            ToggleState();
        }
    }

    private void ToggleState()
    {
        if ( _endLevel.isEventActive )
            return;

        _paused = !_paused;

        Time.timeScale = _paused ? 0.0f : 1.0f;
    }

    private void OnGUI()
    {
        if ( !_paused )
            return;

        //  Need to make sure this is on top so the other screen borders
        //  don't cause issues.
        GUI.depth = 0;
        GUI.color = Color.white * .5f;
        GUI.DrawTexture( new Rect( 0, 0, Screen.width, Screen.height ), _texture );

        GUI.color = Color.white;
        GUI.skin = skin;
        GUI.matrix = _guiMatrix;

        GUILayout.BeginVertical( skin.customStyles[5] );
        GUILayout.Label( "Paused", skin.customStyles[6] );

        if ( GUILayout.Button( "Continue", skin.customStyles[4] ) )
            ToggleState();

        //  We need to remember to switch the game back to normal game
        //  speed before we change the scene.

        if ( GUILayout.Button( "Restart", skin.customStyles[4] ) )
        {
            ToggleState();
            Application.LoadLevel( Application.loadedLevel );
        }


        if ( GUILayout.Button( "Back to Menu", skin.customStyles[4] ) )
        {
            ToggleState();
            Application.LoadLevel( "MainMenu" );
        }

        GUILayout.EndVertical();
    }
}
