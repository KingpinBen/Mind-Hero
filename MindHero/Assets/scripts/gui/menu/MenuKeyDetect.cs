using UnityEngine;
using System.Collections;

public class MenuKeyDetect : MonoBehaviour
{
    public MenuObjectsHandler nextScreen;

    void Awake()
    {
        GameStrings.Load();
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
