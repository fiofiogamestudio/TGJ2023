using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType
{
    Normal,
    Continues,
    Emergency
}

[System.Serializable]
public class MissionData
{
    public MissionType missionType;
    public string name;
    public string description;
    public int amount;
    public int left;
    public string[] costs;
    public string[] turnEffects; // 每回合持续的效果
    public string[] endEffects; // 任务完成的效果
}

[System.Serializable]
public class MissionWrapper
{
    public MissionData[] normalMissions;
    public MissionData[] continuesMissions;
    public MissionData[] emergencyMission;
}

