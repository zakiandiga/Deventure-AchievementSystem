using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] public List<string> OngoingMissions => ongoingMissions;
    [SerializeField] public List<string> FinishedMissions => finishedMissions;

    private Dictionary<string, Mission> availableMissions;
    private List<string> ongoingMissions;
    private List<string> finishedMissions;

    //MonoBehaviour LOOP
    private void Awake()
    {
        availableMissions = CreateAvailableMissionsDictionary();
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
        //check any available missions

        //make missions that satisfy it's requirements ready

        //wait for 'quest point' to make the missions ongoing

        //TEMP mission start
        foreach (Mission mission in availableMissions.Values)
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
    private Dictionary<string, Mission> CreateAvailableMissionsDictionary()
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

    private void AddOngoingMission(string id)
    {
        if(ongoingMissions.Contains(id))
        {
            Debug.LogError("Error, mission is already ongoing!");
            return;
        }

        ongoingMissions.Add(id);
    }

    private void FinishOngoingMission(string id)
    {
        if(!ongoingMissions.Contains(id))
        {
            Debug.LogError("cannot find ongoing mission: " + id);
            return;
        }

        finishedMissions.Add(id);
        ongoingMissions.Remove(id);
    }

    private Mission GetMissionById(string id)
    {
        Mission mission = availableMissions[id];
        if(mission == null)
        {
            Debug.LogError("ID " + id + " not found!");
        }

        return mission;
    }
}
