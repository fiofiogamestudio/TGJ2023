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

    void Awake()
    {
        loadMissions();
    }

    void loadMissions()
    {
        MissionWrapper missionWrapper = DataLoader.LoadJson<MissionWrapper>("Missions");
        NormalMissions = missionWrapper.normalMissions;
        ContinuesMissions = missionWrapper.continuesMissions;
        EmergencyMissions = missionWrapper.emergencyMission;
    }
}
