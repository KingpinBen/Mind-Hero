using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Room))]
public class RoomEditor : Editor
{

    private SerializedProperty _roomName;

    void OnEnable()
    {
        _roomName = serializedObject.FindProperty( "roomType" );
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("This is the " + _roomName.enumNames[_roomName.enumValueIndex] + " room", MessageType.None);
    }

}
