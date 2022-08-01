using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class LevelData
{
    public int level;

    public LevelData(LevelGeneration levels)
    {
        level = levels.level;
    }
}
