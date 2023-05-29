using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameType
{
    Simple,
    Normal,
    Hard
}

[System.Serializable]
public class GameData
{
    public GameType gameType;
    public string name;
    public string descripton;
    public int goodPossibility;
    public int badPossibility;
    public int randomPosiibility;
    public string[] initMissions;
}

[System.Serializable]
public class GameWrapper
{
    public GameData simpleGameData;
    public GameData normalGameData;
    public GameData hardGameData;
}