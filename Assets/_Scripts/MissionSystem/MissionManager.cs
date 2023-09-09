using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> OngoingMissions => ongoingMissions;
    [SerializeField] public List<GameObject> FinishedMissions => finishedMissions;
    [SerializeField] public List<GameObject> AvailableMissions => availableMissions;

    //PRIVATE VARIABLES
    private List<GameObject> availableMissions = new List<GameObject>();
    private List<GameObject> ongoingMissions = new List<GameObject>();
    private List<GameObject> finishedMissions = new List<GameObject>();
    private List<Dictionary<string, object>> missionDataFromCSV;    
    private int tryFetchAttempt = 0;

    //MonoBehaviour
    private void Awake()
    {
        StartCoroutine(TryFetchAvailableMissions());
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    //MISSION EVENT BROADCAST
    public void MissionStart(string id)
    {
        EventManager.Instance.missionEvents.StartMission(id);
        ongoingMissions.Add(GetMissionById(id));
    }

    public void MissionFinish(string id)
    {
        EventManager.Instance.missionEvents.FinishMission(id);
        finishedMissions.Add(GetMissionById(id));
        ongoingMissions.Remove(GetMissionById(id));
    }

    //MISSION MANAGER FUNCTIONS
    private IEnumerator TryFetchAvailableMissions()
    {
        tryFetchAttempt++;
        Debug.Log("Attempt no. " + tryFetchAttempt);

        while (tryFetchAttempt <= 100)
        {
            missionDataFromCSV = CSVReader.Instance.ConvertedData;

            if(missionDataFromCSV != null)
            {
                Debug.Log("Fetch Available Missions Success");
                PopulateMissions();
                break;
            }

            yield return null;
        }

        if(AvailableMissions == null && tryFetchAttempt >= 100)
        {
            Debug.LogError("Failed to fetch available mission from CSVReader");
        }
    }

    private void PopulateMissions()
    {
        Debug.Log("Start Populating missions");

        for (int i = 0; i < missionDataFromCSV.Count; ++i)
        {
            string missionId = (string)missionDataFromCSV[i]["Id"];
            GameObject missionObj = new GameObject(missionId);
            missionObj.transform.parent = transform;

            Mission mission = missionObj.AddComponent<Mission>();
            mission.InitializeMission(
                missionId,
                (string)missionDataFromCSV[i]["MissionName"],
                (int)missionDataFromCSV[i]["Level"],
                (string)missionDataFromCSV[i]["FinishedMission"],
                (string)missionDataFromCSV[i]["EventId"],
                //Set mission type as well
                (string)missionDataFromCSV[i]["Object"],
                (int)missionDataFromCSV[i]["Amount"],
                (string)missionDataFromCSV[i]["Description"]
                );

            availableMissions.Add(missionObj);
        }

        Debug.Log("Populate Missions Complete!");
    }

    public GameObject GetMissionById(string id)
    {
        foreach(GameObject mission in AvailableMissions)
        {
            if (mission.GetComponent<Mission>().MissionId == id)
            {
                return mission;
            }            
        }

        Debug.LogError("FAILED TO GET MISSION BY ID");
        return null;
    }
}
