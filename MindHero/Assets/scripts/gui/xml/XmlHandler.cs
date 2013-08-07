using UnityEngine;
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
            if (value != _locale)
            {
                _locale = value;
                Load();
            }
        }
    }

    private static Locale _locale = Locale.None;
    private static XmlNode _headNode;
    private static string _xmlText = string.Empty;

    private const string LOCALE_FILE_NAME = "MenuText";

    private static void Load()
    {
        var asset = Resources.Load( LOCALE_FILE_NAME ) as TextAsset;
        _xmlText = asset.text;
        
        ReadContents();
        _xmlText = null;
    }

    private static void ReadContents()
    {
        XmlNode currentWorkNode = null;
        var currentWorkingPosition = 0;
        var tagName = "";
        var openingTag = false;
        var foundFirstTag = false;

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

                if (currentWorkNode)
                    currentWorkNode = currentWorkNode.parentNode;
            }
            else
            {
                openingTag = true;

                if (!currentWorkNode)
                {
                    if (foundFirstTag)
                    {
                        currentWorkNode = new XmlNode(null);
                        _headNode = currentWorkNode;
                        currentWorkNode.tagName = tagName;
                    }
                    else
                    {
                        foundFirstTag = true;
                    }
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

                if (currentWorkNode)
                {
                    var content = _xmlText.Substring(currentWorkingPosition + 1, startOfCloseTag - currentWorkingPosition - 1);
                    currentWorkNode.contents = content.Trim();
                }
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

        return SearchNodes( _headNode, tagName, true );
    }

    /// <summary>
    /// The tree of nodes to follow to access a node.
    /// </summary>
    /// <param name="tags">The tag names of the nodes starting from parent tag. Doesn't have to be head of file, just unique</param>
    /// <returns></returns>
    public static XmlNode FindTagWithParentTag(string[] tags)
    {
        var nodes = new XmlNode[tags.Length];
        var currentIndex = 1;
        nodes[0] = _headNode;

        for(; currentIndex < tags.Length; currentIndex++)
        {
            var result = SearchNodes( nodes[currentIndex - 1], tags[currentIndex], true );
            if ( result )
                nodes[currentIndex] = result;
            else
                return null;
        }

        //  We'll only return the final value. That way even it didn't, we'll return null
        return nodes[tags.Length - 1];
    }

    /// <summary>
    /// Pass in the path and get the final node. Don't include header.
    /// This just skips checking unnecessary child nodes.
    /// </summary>
    /// <param name="tags"></param>
    /// <returns></returns>
    public static XmlNode FindNodeWithExactTagsPath(string[] tags)
    {
        var pathlength = tags.Length;
        var currentNode = _headNode;

        for(var i = 0; i < pathlength; i++)
        {
            currentNode = SearchNodes( currentNode, tags[i], false );
            if (currentNode == null)
                return null;
        }

        return currentNode;
    }

    private static XmlNode SearchNodes(XmlNode node, string tag, bool searchChildren)
    {
        if (node.tagName == tag)
            return node;

        if (node.Count == 0)
            return null;

        for(var i = 0; i < node.Count; i++)
        {
            if (node[i].tagName == tag)
                return node[i];

            if (!searchChildren)
                continue;

            var tryValue = SearchNodes(node[i], tag, true);

            if (tryValue)
                return tryValue;
        }

        return null;
    }

    public enum Locale
    {
        None = -1,
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
                Debug.LogException(new UnityException("Index was out of bounds."));
                
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
