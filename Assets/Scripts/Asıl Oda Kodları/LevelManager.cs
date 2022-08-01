using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Singleton

    public static LevelManager instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public GameObject level;
    public GameObject[] enemy;
    public GameObject police;
    public GameObject boss;
}
