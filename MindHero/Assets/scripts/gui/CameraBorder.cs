using UnityEngine;
using System.Collections;

public class CameraBorder : MonoBehaviour
{
    public GUISkin guiSkin;

    private Camera _camera;
    private Rect _rect;

    void Awake()
    {
        XmlHandler.locale = XmlHandler.Locale.EnGB;
    }

    private void Start()
    {
        _camera = camera;

        var pixelRect = _camera.pixelRect;

        _rect = new Rect(
            pixelRect.x,
            (Screen.height - (pixelRect.yMax)),
            _camera.pixelWidth,
            _camera.pixelHeight);
    }

    private void OnGUI()
    {
        GUI.skin = guiSkin;
        GUI.depth = 1;

        GUI.Box(_rect, "");
    }
}
