using UnityEngine;
using System.Collections;

public struct LevelMenuScreenData
{

    public string title;
    public string description;
    public int sceneId;
    public int highScore;

    /// <param name="t">level title</param>
    /// <param name="l">scene id</param>
    /// <param name="d">level description</param>
    /// <param name="s">highscore</param>
    public LevelMenuScreenData(string t, int l, string d, int s)
    {
        title = t;
        description = d;
        highScore = s;
        sceneId = l;
    }
}
