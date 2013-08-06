using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public static class XmlHandler
{
    public static Locale locale
    {
        get
        {
            return _locale;
        }
        set
        {
            if (value == _locale)
                _loaded = false;

            _locale = value;
            Load();
        }
    }

    private static Locale _locale;

    private static XmlNode _headNode;
    private static bool _loaded = false;
    private static string _xmlText = string.Empty;

    private const string LOCALE_FILE_NAME = "MenuText";

    private static void Load()
    {
        if (_loaded)
            return;

        var asset = Resources.Load( LOCALE_FILE_NAME ) as TextAsset;
        _xmlText = asset.text;

        ReadContents();
        _xmlText = null;

        _loaded = true;
    }

    private static void ReadContents()
    {
        XmlNode currentWorkNode = null;
        var currentWorkingPosition = 0;
        var tagName = "";
        var openingTag = false;

        while(true)
        {
            if (currentWorkingPosition > -1)
                currentWorkingPosition = _xmlText.IndexOf('<', currentWorkingPosition);

            //  If it's -1, we're at the end.
            if (currentWorkingPosition == -1)
                return;

            currentWorkingPosition++;

            var endOfTag = _xmlText.IndexOf('>', currentWorkingPosition);
            var endOfName = GetIndex(' ', currentWorkingPosition);

            if (endOfName == -1 || endOfTag < endOfName)
            {
                endOfName = endOfTag;
            }

            if (endOfTag == -1)
                return;

            tagName = _xmlText.Substring(currentWorkingPosition, endOfName - currentWorkingPosition);

            currentWorkingPosition = endOfTag;

            // check if a closing tag
            if (tagName.StartsWith("/"))
            {
                openingTag = false;
                tagName = tagName.Remove(0, 1); // remove the slash
                currentWorkNode = currentWorkNode.parentNode;
            }
            else
            {
                openingTag = true;

                if (!currentWorkNode)
                {
                    currentWorkNode = new XmlNode(null);
                    _headNode = currentWorkNode;
                    currentWorkNode.tagName = tagName;
                }
                else
                {
                    currentWorkNode = currentWorkNode.AddChild(tagName);
                }
            }

            // if an opening tag, get the content
            if (openingTag)
            {
                int startOfCloseTag = _xmlText.IndexOf("<", currentWorkingPosition);
                if (startOfCloseTag == -1)
                {
                    return;
                }

                var content = _xmlText.Substring(currentWorkingPosition + 1, startOfCloseTag - currentWorkingPosition - 1);
                currentWorkNode.contents = content.Trim();
            }
        }
    }

    private static int GetIndex(char c, int startPoint)
    {
        for ( var i = startPoint; i < _xmlText.Length; i++ )
        {
            if ( _xmlText[i] == c )
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Finds the first node in the tree that uses the tag name passed in.
    /// </summary>
    /// <param name="tagName">The tag to search for</param>
    /// <returns>The node named as the tagged parameter.</returns>
    public static XmlNode FindNodeWithTagName(string tagName)
    {
        if (!_headNode)
            return null;

        return SearchNodes( _headNode, tagName );
    }

    /// <summary>
    /// The tree of nodes to follow to access a node.
    /// </summary>
    /// <param name="tags">The tag names of the nodes starting from parent tag. Doesn't have to be head of file, just unique</param>
    /// <returns></returns>
    public static XmlNode FindTagWithParentTag(string[] tags)
    {
        var nodes = new XmlNode[tags.Length];
        var currentIndex = 0;
        nodes[currentIndex] = FindNodeWithTagName( tags[currentIndex++] );

        if (!nodes[0])
            return null;

        for(; currentIndex < tags.Length; currentIndex++)
        {
            var result = SearchNodes( nodes[currentIndex - 1], tags[currentIndex] );
            if ( result )
                nodes[currentIndex] = result;
            else
                return null;
        }

        //  We'll only return the final value. That way even it didn't, we'll return null
        return nodes[tags.Length - 1];
    }

    private static XmlNode SearchNodes(XmlNode node, string tag)
    {
        if (node.tagName == tag)
            return node;

        if (node.Count == 0)
            return null;

        for(var i = 0; i < node.Count; i++)
        {
            if (node[i].tagName == tag)
                return node[i];

            var tryValue = SearchNodes(node[i], tag);

            if (tryValue)
                return tryValue;
        }

        return null;
    }

    public enum Locale
    {
        EnGB
    }
}

public class XmlNode
{
    public readonly XmlNode parentNode;
    public string tagName;
    public string contents;

    private readonly List<XmlNode> _childNodes = new List<XmlNode>(); 

    /// <summary>
    /// Returns the child node at index if it exists.
    /// </summary>
    /// <param name="index">Index to return</param>
    /// <returns>Node at index</returns>
    public XmlNode this[int index]
    {
        get
        {
            if (index < 0 || index >= _childNodes.Count)
                throw new Exception("Index was out of bounds.");
                
            return _childNodes[index];
        }
    }

    /// <summary>
    /// The amount of child nodes the current node has attached.
    /// </summary>
    public int Count
    {
        get
        {
            return _childNodes.Count;
        }
    }

    public XmlNode(XmlNode parent)
    {
        parentNode = parent;
    }

    public XmlNode AddChild(string tag)
    {
        if (string.IsNullOrEmpty(tag))
            Debug.LogWarning("Tried adding a bad tag to: '"+ tagName + "'");

        var newNode = new XmlNode( this )
                          {
                              tagName = tag
                          };
        _childNodes.Add(newNode);

        return newNode;
    }

    public List<XmlNode> GetChildren()
    {
        return _childNodes;
    } 

    public static implicit operator bool(XmlNode node)
    {
        return node != null;
    }
}
