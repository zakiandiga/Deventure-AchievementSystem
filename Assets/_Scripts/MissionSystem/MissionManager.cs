using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public List<GameObject> AvailableMissions => availableMissions;

    //PRIVATE VARIABLES
    [SerializeField] private List<GameObject> availableMissions = new List<GameObject>();
    private List<Dictionary<string, string>> missionDataFromCSV;    
    private int tryFetchAttempt = 0;

    private bool canEvaluate = false;
    private bool hasData = false;
    private readonly float evaluationRate = 0.7f;

    //FETCH FROM CSV & INITIALIZATION
    private void StartFetchingData(string docId)
    {
        if (!hasData)
        {
            StartCoroutine(TryFetchAvailableMissions(docId));
        }
    }

    private IEnumerator TryFetchAvailableMissions(string docId)
    {
        CSVReader.Instance.InitiateDownloadSheet(docId);
        
        tryFetchAttempt++;

        while (tryFetchAttempt <= 100)
        {
            missionDataFromCSV = CSVReader.Instance.ConvertedData;

            if (missionDataFromCSV != null)
            {
                Debug.Log("Fetch Available Missions Success");
                hasData = true;
                PopulateMissions();
                break;
            }

            yield return null;
        }

        if (AvailableMissions == null && tryFetchAttempt >= 100)
        {
            Debug.LogError("Failed to fetch available mission from CSVReader");
        }
    }    

    private void PopulateMissions()
    {
        Logger.UIMessage("Start populating mission");

        for (int i = 0; i < missionDataFromCSV.Count; ++i)
        {
            string missionId = (string)missionDataFromCSV[i]["id"];
            string unlocked = (missionDataFromCSV[i]["unlocked"] as string) ?? "0";

            GameObject missionObject = new GameObject(missionId);
            missionObject.transform.parent = this.transform;

            Mission mission = missionObject.AddComponent<Mission>();
            mission.InitializeMission(missionId, unlocked, missionDataFromCSV[i]["description"]);

            for (int j = 1; ; j++)
            {
                string lhsKey = $"lhs{j}";
                string opKey = $"op{j}";
                string rhsKey = $"rhs{j}";

                if (!missionDataFromCSV[i].ContainsKey(lhsKey)) break;
                if (string.IsNullOrEmpty((string)missionDataFromCSV[i][lhsKey])) break;
                
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
        }
 
        Logger.UIMessage(availableMissions.Count + " Missions populated");

        canEvaluate = true;
        StartCoroutine(EvaluationLoop());
    }  

    //MISSION MANAGER FUNCTIONS
    private void EvaluateReadyMissions()
    {
        if(availableMissions.Count <= 0)
        {
            Debug.LogWarning("No mission available!");
            return;
        }

        for (int i = 0; i < availableMissions.Count; i++)
        {
            Mission mission = availableMissions[ i ].GetComponent< Mission >();

            if (mission.Unlocked == 0) continue;

            mission.Evaluate();
        }
    }

    public GameObject GetMissionById(string id)
    {
        foreach(GameObject mission in AvailableMissions)
        {
            if (mission.GetComponent<Mission>().Id == id)
            {
                return mission;
            }            
        }

        Debug.LogError("FAILED TO GET MISSION BY ID");
        return null;
    }

    //MONO BEHAVIOUR
    private void Awake()
    {
        UIManager.OnStartPressed += StartFetchingData;
        UIManager.OnPausePressed += PauseEvaluation;
    }

    private void OnDestroy()
    {
        UIManager.OnStartPressed -= StartFetchingData;
        UIManager.OnPausePressed -= PauseEvaluation;
        StopCoroutine(EvaluationLoop());
    }

    private void PauseEvaluation(bool value)
    {
        canEvaluate = !value;

        string message = !canEvaluate ? "Evaluation paused!" : "Evaluation continued!";
        Logger.UIMessage(message);
    }

    private IEnumerator EvaluationLoop()
    {
        while (true)
        {
            if (canEvaluate)
            {
                EvaluateReadyMissions();
            }

            yield return new WaitForSeconds(evaluationRate);
        }
    }
}
