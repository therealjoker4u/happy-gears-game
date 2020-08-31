using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsList : MonoBehaviour
{

    public static List<string> levels;
    public static int numberOfLevels = 36;
    public static void init()
    {
        List<string> newList = new List<string>();
        for(int i = 1; i <= numberOfLevels ; i++)
        {
            string indexStr = $"{i}";
            if (i < 10)
                indexStr = $"0{indexStr}";
            newList.Add($"Level {indexStr}");
        }
        levels = newList;
    }

}
