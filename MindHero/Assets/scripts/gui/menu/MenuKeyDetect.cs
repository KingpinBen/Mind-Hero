using UnityEngine;
using System.Collections;

public class MenuKeyDetect : MonoBehaviour
{
    public MenuObjectsHandler nextScreen;

    void Awake()
    {
        XmlHandler.locale = XmlHandler.Locale.EnGB;
        SaveManager.instance.Load();
    }

    void Start()
    {
    }

    void OnGUI()
    {
        if (Event.current.type == EventType.MouseDown || 
            Event.current.type == EventType.KeyDown)
        {
            OnKeyPress();
        }
    }

    void OnKeyPress()
    {
        gameObject.SetActive(false);
        nextScreen.SetState(true, false, null);
    }
}
