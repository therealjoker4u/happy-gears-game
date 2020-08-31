using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LevelData
{
    public string name;
    public int stars;
    public LevelData(string Name, int Stars)
    {
        name = Name;
        stars = Stars;
    }
}

[System.Serializable]
public class LevelsModel
{
    public Dictionary<string, LevelData> data;
    public LevelsModel()
    {
        data = new Dictionary<string, LevelData>();
    }
}
