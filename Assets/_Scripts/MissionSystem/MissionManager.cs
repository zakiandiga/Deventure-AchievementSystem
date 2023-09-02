using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    private Dictionary<string, Mission> missionsDictionary;

    //MonoBehaviour LOOP
    private void Awake()
    {
        missionsDictionary = CreateMissionsDictionary();
    }

    private void OnEnable()
    {
        EventManager.Instance.missionEvents.OnMissionStarted += StartMission;
        EventManager.Instance.missionEvents.OnMissionFinished += FinishMission;
        EventManager.Instance.missionEvents.OnMissionAdvanced += AdvanceMission;
    }

    private void OnDisable()
    {
        EventManager.Instance.missionEvents.OnMissionStarted -= StartMission;
        EventManager.Instance.missionEvents.OnMissionFinished -= FinishMission;
        EventManager.Instance.missionEvents.OnMissionAdvanced -= AdvanceMission;
    }

    private void Start()
    {
        foreach (Mission mission in missionsDictionary.Values)
        {
            EventManager.Instance.missionEvents.MissionStateChange(mission);
        }
    }


    //DELEGATES
    private void StartMission(string id)
    {
        Mission mission = GetMissionById(id);
        mission.InstantiateCurrentMissionSequence(this.transform);
        Debug.Log(mission.GetMissionInfo().displayName + " IS STARTED");
    }

    private void FinishMission(string id)
    {
        Mission mission = GetMissionById(id);

        Debug.Log(mission.GetMissionInfo().displayName + " IS FINISHED");
    }

    private void AdvanceMission(string id)
    {
        Mission mission = GetMissionById(id);
        mission.AdvanceMissionSequence();
        if(!mission.NextSequenceAvailable())
        {
            FinishMission(id);
            return;
        }

        mission.InstantiateCurrentMissionSequence(this.transform);
    }

    //MISSION MANAGER FUNCTIONS
    private Dictionary<string, Mission> CreateMissionsDictionary()
    {
        MissionInfo[] missionInfos = Resources.LoadAll<MissionInfo>("Missions");

        Dictionary<string, Mission> tempDictionary = new Dictionary<string, Mission>();

        foreach (MissionInfo missionInfo in missionInfos)
        {
            if(tempDictionary.ContainsKey(missionInfo.id))
            {
                Debug.LogWarning("Duplicated mission ID found");
            }

            Mission missionToAdd = new Mission(missionInfo);
            tempDictionary.Add(missionInfo.id, missionToAdd);
        }

        return tempDictionary;
    }

    private Mission GetMissionById(string id)
    {
        Mission mission = missionsDictionary[id];
        if(mission == null)
        {
            Debug.LogError("ID " + id + " not found!");
        }

        return mission;
    }
}
