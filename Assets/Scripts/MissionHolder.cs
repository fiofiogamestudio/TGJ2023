using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionHolder : MonoBehaviour
{
    [Header("Missions Info")]
    [ReadOnly]
    public MissionData[] NormalMissions;
    [ReadOnly]
    public MissionData[] ContinuesMissions;
    [ReadOnly]
    public MissionData[] EmergencyMissions;

    public static MissionHolder instance;
    void Awake()
    {
        if (instance == null) instance = this;
        loadMissions();
    }

    void loadMissions()
    {
        MissionWrapper missionWrapper = DataLoader.LoadJson<MissionWrapper>("Missions");
        NormalMissions = missionWrapper.normalMissions;
        ContinuesMissions = missionWrapper.continuesMissions;
        EmergencyMissions = missionWrapper.emergencyMission;
    }

    public MissionData GetMissionData(string name)
    {
        foreach (var mission in NormalMissions)
        {
            if (mission.name == name) return mission;
        }
        foreach (var mission in ContinuesMissions)
        {
            if (mission.name == name) return mission;
        }
        foreach (var mission in EmergencyMissions)
        {
            if (mission.name == name) return mission;
        }
        return null;
    }
}
