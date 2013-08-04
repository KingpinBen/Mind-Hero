using UnityEngine;
using UnityEditor;
using System;

public class AutoSave : EditorWindow
{

    private bool _autoSaveScene = true;
    private bool _showMessage = true;
    private bool _isStarted;
    private int _intervalScene;
    private DateTime _lastSaveTimeScene = DateTime.Now;

    private readonly string _projectPath = Application.dataPath;
    private string _scenePath;

    [MenuItem("Window/AutoSave")]
    private static void Init()
    {
        var saveWindow = GetWindow(typeof (AutoSave)) as AutoSave;
        if (saveWindow != null) saveWindow.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Info:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Saving to:", "" + _projectPath);
        EditorGUILayout.LabelField("Saving scene:", "" + _scenePath);
        GUILayout.Label("Options:", EditorStyles.boldLabel);
        _autoSaveScene = EditorGUILayout.BeginToggleGroup("Auto save", _autoSaveScene);
        _intervalScene = EditorGUILayout.IntSlider("Interval (minutes)", _intervalScene, 1, 10);

        if (_isStarted)
            EditorGUILayout.LabelField("Last save:", "" + _lastSaveTimeScene);

        EditorGUILayout.EndToggleGroup();
        _showMessage = EditorGUILayout.BeginToggleGroup("Show Message", _showMessage);
        EditorGUILayout.EndToggleGroup();
    }


    private void Update()
    {
        _scenePath = EditorApplication.currentScene;
        if (_autoSaveScene)
        {
            if (DateTime.Now.Minute >= (_lastSaveTimeScene.Minute + _intervalScene) ||
                DateTime.Now.Minute == 59 && DateTime.Now.Second == 59)
                SaveScene();
        }
        else
            _isStarted = false;
    }

    private void SaveScene()
    {
        EditorApplication.SaveScene(_scenePath);
        _lastSaveTimeScene = DateTime.Now;
        _isStarted = true;
        if (_showMessage)
            Debug.Log("AutoSave saved: " + _scenePath + " on " + _lastSaveTimeScene);

        var repaintSaveWindow = GetWindow(typeof (AutoSave)) as AutoSave;
        if (repaintSaveWindow != null) repaintSaveWindow.Repaint();
    }
}