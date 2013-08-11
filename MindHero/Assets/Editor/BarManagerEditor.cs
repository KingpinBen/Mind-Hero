using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BarManager))]
public class BarManagerEditor : Editor
{

    private SerializedProperty _blockSpeed;

    void OnEnable()
    {
        _blockSpeed = serializedObject.FindProperty( "blockSpeed" );
    }

    public override void OnInspectorGUI()
    {
        _blockSpeed.floatValue = EditorGUILayout.Slider( "Block Speed", _blockSpeed.floatValue, 1, 10 );

        serializedObject.ApplyModifiedProperties();
    }
}
