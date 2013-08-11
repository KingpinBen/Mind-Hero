using UnityEngine;
using UnityEditor;

[CustomEditor( typeof ( Worker ) )]
public class WorkerEditor : Editor
{
    private SerializedProperty _movementSpeed;

    void OnEnable()
    {
        _movementSpeed = serializedObject.FindProperty( "movementSpeed" );
    }

    public override void OnInspectorGUI()
    {
        _movementSpeed.floatValue = EditorGUILayout.Slider( "Movement Speed", _movementSpeed.floatValue, .3f, 3.0f );
        serializedObject.ApplyModifiedProperties();
    }
}
