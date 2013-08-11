using UnityEngine;
using System.Collections;

public class CameraBorder : MonoBehaviour
{
    public GUISkin guiSkin;

    void Awake()
    {
        XmlHandler.locale = XmlHandler.Locale.EnGB;
    }

    private void OnGUI()
    {
        GUI.skin = guiSkin;
        GUI.depth = 1;
        var cameras = Camera.allCameras;

        for (var i = 0; i < cameras.Length; i++)
        {
            var pixelRect = cameras[i].pixelRect;
            GUI.Box( new Rect(
                         pixelRect.x,
                         ( Screen.height - ( pixelRect.yMax ) ),
                         cameras[i].pixelWidth,
                         cameras[i].pixelHeight), "");
        }
    }
}
