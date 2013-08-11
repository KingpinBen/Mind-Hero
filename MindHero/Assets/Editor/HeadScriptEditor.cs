using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(HeadScript))]
public class HeadScriptEditor : Editor {

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox( "This is the head script! Nothing to change here", MessageType.Info );
    }
}
