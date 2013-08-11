using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WildcardRoom))]
public class WildcardRoomEditor : Editor
{

    private SerializedProperty _regenRate;
    private SerializedProperty _degenRate;

    void OnEnable()
    {
        _regenRate = serializedObject.FindProperty("regenerationWorkerRate");
        _degenRate = serializedObject.FindProperty("degenerationRate");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Regeneration rate is based on how many workers are currently in the room.", MessageType.Info);
        _regenRate.floatValue = EditorGUILayout.FloatField( "Regeneration Rate", _regenRate.floatValue );
        _degenRate.floatValue = EditorGUILayout.FloatField( "Degeneration Rate", _degenRate.floatValue );

        serializedObject.ApplyModifiedProperties();
    }
}
