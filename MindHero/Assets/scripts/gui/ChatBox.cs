using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;
using System.Text;

public class ChatBox : MonoBehaviour
{
    private const float TIME_BETWEEN_LETTERS = 0.001f;

    public TextAsset document;
    public GUIStyle style;

    private string _sceneName;
    private XmlDocument _xmlDocument;
    private Camera _chatCamera;
    private int _letterCounter;
    private string _message = string.Empty;
    private float _elapsed;
    private float _windowWidth;
    private bool _finishedWriting;
    private StringBuilder _stringBuilder;
    private Rect _rect;

    void Awake()
    {
        _xmlDocument = new XmlDocument();
        _xmlDocument.LoadXml(document.text);

        _chatCamera = Camera.main;
        _windowWidth = Screen.width * _chatCamera.rect.width;
    }

    void Start()
    {
        _stringBuilder = new StringBuilder();
    }

    void Update()
    {
        if (_message != "")
            if (_finishedWriting == false)
            {
                _elapsed += Time.deltaTime;

                if (_elapsed >= TIME_BETWEEN_LETTERS)
                {
                    _elapsed = 0.0f;

                    _stringBuilder.Append(_message[_letterCounter]);

                    _letterCounter++;
                }

                if (_stringBuilder.ToString() == _message)
                    _finishedWriting = true;
            }
    }

    public void SetMessageFromId(string textId, object sender)
    {
        var text = string.Empty;
        var sentMessage = false;

        //  Reset some values.
        _finishedWriting = false;
        _letterCounter = 0;
        _elapsed = 0.0f;
        _stringBuilder = new StringBuilder();
        _message = string.Empty;

        try
        {
            text = _xmlDocument.SelectSingleNode("sceneChatStrings/messages/message[@id='" + textId + "']/text").InnerText;
            //var pic = _xmlDocument.SelectSingleNode("sceneChatStrings/messages/message[@id='" + textID + "']/comicNodeID").InnerText;
        }
        catch
        {
            Debug.LogWarning("Couldn't find the referenced ID!");
        }

        try
        {
            var arrayNode = _xmlDocument.SelectSingleNode("sceneChatStrings" + "/messages/message[@id='" + textId + "']/chatarray");
            
            if (arrayNode != null)
            {
                var arrayNodes = arrayNode.ChildNodes;
                int i;

                var words = new string[arrayNodes.Count];

                //  The amount of arrays in the event
                for (i = 0; i < arrayNodes.Count; i++)
                {
                    var arrayValueCount = arrayNodes[i].ChildNodes.Count;
                    words[i] =
                        arrayNodes[i].ChildNodes[UnityEngine.Random.Range(0, arrayValueCount)].InnerText;
                }

                SingleLineMessage(string.Format(text, words));
                sentMessage = true;
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        if (sentMessage == false)
            SingleLineMessage(text);
    }
    public void SetFinished()
    {
        _finishedWriting = true;
        _stringBuilder = new StringBuilder(_message);
    }
    public bool GetFinished()
    {
        return _finishedWriting;
    }

    void OnGUI()
    {
        GUI.Box(
            _rect,
            _stringBuilder.ToString(),
            style);

    }

    void SingleLineMessage(string text)
    {
        var windowBottomLeft = new Vector2(Screen.width * _chatCamera.rect.x, Screen.height - (Screen.height * _chatCamera.rect.y));
        _rect = new Rect(windowBottomLeft.x, windowBottomLeft.y, 30, 30);

        //  Measuring the pixel length of the string fucks up if you don't remove the padding,
        //  so backup and set 0's.
        var backupRect = new RectOffset(style.padding.left, style.padding.right, style.padding.top, style.padding.bottom);
        style.padding = new RectOffset(0, 0, 0, 0);

        //  Set up some locals
        var lineCounter = 1;
        float lineLength = 0;
        var maxChatWidth = _windowWidth - (backupRect.right + backupRect.left);

        var words = text.Split(new string[] { " " }, StringSplitOptions.None);

        const float spaceWidth = 5;

        for (var i = 0; i < words.Length; i++)
        {
            var wordLength = style.CalcSize(new GUIContent(words[i]));
            if (lineLength + wordLength.x > maxChatWidth)
            {
                lineCounter++;
                break;
            }

            lineLength += wordLength.x;

            if (i + 1 != words.Length)
                lineLength += spaceWidth;
        }

        if (lineLength < 50)
            lineLength += 12f;
        else if (lineCounter > 1)
            lineLength = _windowWidth - 4;

        style.padding = backupRect;

        var height = 18 * lineCounter;
        var y = height + 2;

        _rect = new Rect(windowBottomLeft.x + 2,
            windowBottomLeft.y - y,
            lineLength, height);

        _message = text;
    }
}
