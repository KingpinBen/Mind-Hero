using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BankRoom))]
public class BankRoomEditor : Editor
{

    private SerializedProperty _colors;
    private SerializedProperty _respawnTimer;

    void OnEnable()
    {
        _colors = serializedObject.FindProperty( "workerColours" );
        _respawnTimer = serializedObject.FindProperty( "respawnTimer" );
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox( "Each Brain blob will randomly choose one of these colours", MessageType.Info );
        EditorGUILayout.PropertyField( _colors, true );
        _respawnTimer.floatValue = EditorGUILayout.Slider( "Respawn Timer (sec)", _respawnTimer.floatValue, .5f, 10 );

        serializedObject.ApplyModifiedProperties();
    }
}
