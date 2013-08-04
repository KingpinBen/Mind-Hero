using UnityEngine;
using System.Collections;

public struct LevelMenuScreenData
{

    public string title;
    public string description;
    public string sceneName;

    public int highScore;

    /// <param name="t">level title</param>
    /// <param name="l">scene name</param>
    /// <param name="d">level description</param>
    /// <param name="s">highscore</param>
    public LevelMenuScreenData(string t, string l, string d, int s)
    {
        title = t;
        description = d;
        highScore = s;
        sceneName = l;
    }
}
