using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PlayerSpeedTrigger))]
public class PlayerSpeedEditor : Editor
{

    private SerializedProperty _speed;

    void OnEnable()
    {
        _speed = serializedObject.FindProperty("speed");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        _speed.floatValue = 
            EditorGUILayout.Slider("Target Speed", _speed.floatValue, 0, 1);

        serializedObject.ApplyModifiedProperties();
    }
}
