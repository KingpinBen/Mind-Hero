using UnityEngine;
using System.Collections;

public class MenuLevelSelectGui : MonoBehaviour
{

    public GUISkin skin;

    private Camera _camera;
    private int _objectHeight;
    private int _objectWidth;
    private const int OBJECT_Y_OFFSET = 10;

    private int _hoveredOver = -1;

    void Start()
    {
        _camera = Camera.main;
        var tex = skin.customStyles[0].normal.background;
        _objectHeight = tex.height;
        _objectWidth = tex.width;
    }

    void MouseDownEvent()
    {
        if (_hoveredOver == -1) return;
        switch(_hoveredOver)
        {
            case 0:
                PlayerPrefs.SetFloat("startMovementSpeed", 0.2f);
                Application.LoadLevel(1);
                break;
            case 1:
                PlayerPrefs.SetFloat("startMovementSpeed", 0.7f);
                Application.LoadLevel(2);
                break;
            case 2:
                PlayerPrefs.SetFloat("startMovementSpeed", 1f);
                Application.LoadLevel(3);
                break;
        }
        
    }

    void OnGUI()
    {
        GUI.skin = skin;
        int i;
        var scale = Screen.height*0.001f;

        _hoveredOver = -1;

        GUI.matrix = Matrix4x4.TRS(
            new Vector3(
                (_camera.pixelWidth * 0.5f) - ((_objectWidth * scale) * 0.5f), 
                (_camera.pixelHeight * 0.5f - ((_objectHeight * 1.5f) + OBJECT_Y_OFFSET) * scale), 
                0), Quaternion.identity, new Vector3(scale, scale, 1));

        for (i = 0; i < skin.customStyles.Length; i++ )
        {
            var rect = new Rect(0, (OBJECT_Y_OFFSET + _objectHeight) * i, _objectWidth, _objectHeight);
            if (GUI.Button(rect, "", skin.customStyles[i].name))
            {
                _hoveredOver = i;
                MouseDownEvent();
            }
        }
    }
}
