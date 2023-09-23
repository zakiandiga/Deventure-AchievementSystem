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
    private List<Dictionary<string, string>> missionDataFromCSV;    
    private int tryFetchAttempt = 0;

    //MonoBehaviour
    private void Awake()
    {
        //StartCoroutine(TryFetchAvailableMissions());
        StartCoroutine(TryFetchAvailableMissions02());
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

        CheckAllComplete();
    }

    //MISSION MANAGER FUNCTIONS

    private IEnumerator TryFetchAvailableMissions02()
    {
        tryFetchAttempt++;
        Debug.Log("Attempt no. " + tryFetchAttempt);

        while (tryFetchAttempt <= 100)
        {
            missionDataFromCSV = CSVReader.instance.ConvertedData;

            if (missionDataFromCSV != null)
            {
                Debug.Log("Fetch Available Missions Success");
                PopulateMissions02();
                break;
            }

            yield return null;
        }

        if (AvailableMissions == null && tryFetchAttempt >= 100)
        {
            Debug.LogError("Failed to fetch available mission from CSVReader");
        }
    }

    private void PopulateMissions02()
    {
        Debug.Log("Start populating mission 02");
        //string description = (string)missionDataFromCSV[1]["description"];


        for(int i = 0; i < missionDataFromCSV.Count; ++i)
        {
            string missionId = (string)missionDataFromCSV[i]["id"];
            GameObject missionObject = new GameObject(missionId);
            missionObject.transform.parent = this.transform;

            Mission02 mission = missionObject.AddComponent<Mission02>();
            mission.InitializeMission(missionDataFromCSV[i]["id"], missionDataFromCSV[i]["unlocked"], missionDataFromCSV[i]["description"]);

            for (int j = 1; ; j++)
            {
                string lhsKey = $"lhs{j}";
                string opKey = $"op{j}";
                string rhsKey = $"rhs{j}";

                if (!missionDataFromCSV[i].ContainsKey(lhsKey))
                {
                    //Debug.Log("row " + i + " doesn't have " + lhsKey);
                    break;
                }

                if (string.IsNullOrEmpty((string)missionDataFromCSV[i][lhsKey]))
                {
                    //Debug.Log("row " + i + " " + lhsKey + " doesn't have value!!");
                    break;
                }

                string lhsValue = missionDataFromCSV[i][lhsKey];
                string opValue = missionDataFromCSV[i][opKey];
                string rhsValue = missionDataFromCSV[i][rhsKey];
                if(string.IsNullOrEmpty(rhsValue))
                {
                    rhsValue = "NULL";
                }

                mission.AddMissionAction(lhsValue, opValue, rhsValue);
                
            }

            availableMissions.Add(missionObject);
            mission.PrintMission();
        }        
    }  

    private void CheckAllComplete()
    {
        if(FinishedMissions.Count >= AvailableMissions.Count)
        {
            Debug.Log("All mission finished!");
        }
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
