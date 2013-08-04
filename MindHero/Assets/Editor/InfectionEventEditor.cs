using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

[CustomEditor(typeof(InfectionEvent))]
public class InfectionEventEditor : Editor
{
    private SerializedProperty _rateChange;
    private SerializedProperty _targetRoom;
    private SerializedProperty _infectionRate;
    private SerializedProperty _setInfectionLevel;
    private SerializedProperty _textToDisplay;
    private SerializedProperty _infectionEvent;

    private Enum _type;

    void OnEnable()
    {
        _rateChange = serializedObject.FindProperty("rateChange");
        _infectionRate = serializedObject.FindProperty("infectionRate");
        _targetRoom = serializedObject.FindProperty("targetRoom");
        _setInfectionLevel = serializedObject.FindProperty("infectionValue");
        _textToDisplay = serializedObject.FindProperty("triggerText");
        _infectionEvent = serializedObject.FindProperty("endInfectionOnCleared");
    }

    public override void OnInspectorGUI()
    {
        _infectionEvent.boolValue = EditorGUILayout.Toggle("Infection Event", _infectionEvent.boolValue);
        _rateChange.boolValue = EditorGUILayout.BeginToggleGroup("Set New Infection Value", _rateChange.boolValue);
        EditorGUILayout.HelpBox("-1=Bad, 0-Infection Start, 1=Cleanest", MessageType.Info);
        _setInfectionLevel.floatValue =
            EditorGUILayout.Slider("New Infection Value", _setInfectionLevel.floatValue,
                                   -1f, 1f);

        EditorGUILayout.EndToggleGroup();

        _infectionRate.floatValue =
            EditorGUILayout.Slider("Infection Rate", _infectionRate.floatValue, 0, 15);

        _type = (WildcardRoomType) _targetRoom.enumValueIndex;
        _type = EditorGUILayout.EnumPopup("Targetted Room", _type);
        _targetRoom.enumValueIndex = _type.GetHashCode();
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.HelpBox(
            "The message here can either be a custom message or one from the locale folder (preferable) using the key name of the message.\nE.g. EYES_INFECT_1",
            MessageType.Info);
        _textToDisplay.stringValue = EditorGUILayout.TextField("Message", _textToDisplay.stringValue);
    }
}
