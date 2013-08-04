using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(BarPushTrigger))]
public class BarPushTriggerEditor : Editor
{
    public SerializedProperty columns;
	public SerializedProperty character;
    public SerializedProperty message;

    void OnEnable()
    {
        columns = serializedObject.FindProperty("columns");
		character = serializedObject.FindProperty("character");
        message = serializedObject.FindProperty("message");
    }

    public override void OnInspectorGUI()
    {
        var trig = serializedObject.targetObject as BarPushTrigger;

        character.objectReferenceValue =
            EditorGUILayout.ObjectField("Trigger's Character",
                                        character.objectReferenceValue, typeof (AiCharacter), true);

        EditorGUILayout.Separator();

        EditorGUILayout.HelpBox(
            "Info:\nLeave 'Message On Trigger' empty to not display a message.",
            MessageType.Info);

        message.stringValue =
            EditorGUILayout.TextField("Message on Trigger", message.stringValue);

        EditorGUILayout.Separator();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Block Group Size ");
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.Width(20)))
            columns.arraySize++;

        if (GUILayout.Button("Fix", EditorStyles.miniButtonMid))
            Fix(trig);

        if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(20)) && columns.arraySize > 1)
            columns.arraySize--;

        serializedObject.ApplyModifiedProperties();

        GUILayout.EndHorizontal();

        if (trig.columns.Length > 0)
        {
            for (uint y = 0; y < 6; y++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label((RoomType)y + " Bar ", GUILayout.MaxWidth(80));
                
                for (var x = 0; x < trig.columns.Length; x++)
                {
                    if (trig.columns[x].rows == null)
                        trig.columns[x].rows = new bool[6];

                    trig.columns[x].rows[y] =
                        GUILayout.Toggle(trig.columns[x].rows[y], new GUIContent());
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    void Fix(BarPushTrigger trig)
    {
        var tempArray = new BlockColumnArray[trig.columns.Length];

        for (int i = 0; i < tempArray.Length; i++)
        {
            if (tempArray[i] == null)
                tempArray[i] = new BlockColumnArray();

            for (int r = 0; r < 6; r++)
            {
                tempArray[i].rows[r] = trig.columns[i].rows[r];
            }
        }

        trig.columns = tempArray;
    }
}
