using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomBar))]
public class RoomBarEditor : Editor
{

    private SerializedProperty _color;
    private SerializedProperty _room;

    void OnEnable()
    {
        _color = serializedObject.FindProperty("blockColor");
        _room = serializedObject.FindProperty("room");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox( "This is the " + _room.objectReferenceValue.name + " room's bar",
                                 MessageType.None );

        _color.colorValue = EditorGUILayout.ColorField("Block color", _color.colorValue);

        serializedObject.ApplyModifiedProperties();
    }
}
