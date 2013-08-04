using System;
using UnityEngine;
using System.Collections;
using System.Xml;

public static class GameStrings
{
    public static string language = "english";

    //  The file-name of the xml resource to be loaded.
    private const string FILE_LOCATION = "MenuText";
    private static string _xmlStartDirectory;
    private static XmlDocument _stringDoc;

    /// <summary>
    /// Load the XmlDocument if it hasn't already been loaded.
    /// </summary>
    public static void Load()
    {
        if (_stringDoc == null)
        {
            _xmlStartDirectory = "localeData/" + language + "/";

            _stringDoc = new XmlDocument();

            var asset = Resources.Load(FILE_LOCATION) as TextAsset;

            if (asset)
                _stringDoc.LoadXml(asset.text);
            else
                throw new Exception("Couldn't find the file location of the locale.");
        }
    }

    /// <summary>
    /// Returns the text from the XML node passed in param
    /// </summary>
    /// <param name="nodeLocation">The path of the Node in the XML document.</param>
    /// <returns>The text value</returns>
    public static string GetNodeString(string nodeLocation)
    {
        if (_stringDoc == null)
            Load();

        var selectSingleNode = _stringDoc.SelectSingleNode(_xmlStartDirectory + nodeLocation);
        if (selectSingleNode != null)
            return selectSingleNode.InnerText;

        return "ERROR";
    }
}
