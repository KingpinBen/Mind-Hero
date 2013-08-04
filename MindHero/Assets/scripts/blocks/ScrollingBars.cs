using UnityEngine;
using System.Text;

public class ScrollingBars : MonoBehaviour
{
    public GUIStyle style;

    private StringBuilder _stringBuilder;
    private Camera _camera;
    private Rect _rectangle;
    private Vector2 _windowTopLeft;

    private void Awake()
    {
        _camera = GameObject.FindWithTag("GUICamera").GetComponent<Camera>();

        _windowTopLeft = new Vector2(
            Screen.width*_camera.rect.xMin,
            Screen.height*(1 - _camera.rect.yMax));
        _rectangle = new Rect(_windowTopLeft.x, _windowTopLeft.y,
                              100, Screen.height*_camera.rect.yMax);

        _stringBuilder = new StringBuilder();
        _stringBuilder.Append("Nose\n" +
                              "Mouth\n" +
                              "Eyes\n" +
                              "Memory\n" +
                              "Speech\n" +
                              "Think\n" +
                              "Move");

        style.fontSize = (int) ((Screen.height*.1f)*_camera.rect.yMax);
    }

    private void OnGUI()
    {
        GUI.Box(_rectangle, _stringBuilder.ToString(), style);
    }
}
