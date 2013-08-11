using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HashIDs))]
public class HashIdEditor : Editor {

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(
            "Used by Characters to quickly access their animator. You can't change anything here, son.",
            MessageType.None );
    }
}
