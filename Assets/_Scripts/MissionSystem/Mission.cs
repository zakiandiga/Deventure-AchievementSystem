using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Mission : MonoBehaviour
{
    //Properties
    public string MissionId => missionId;
    public string MissionName => missionName;
    public int LevelRequirement => levelRequirement;
    public string FinishedMissionsId => finishedMissionId;
    public string EventId => eventId;
    public string ObjectName => objectName;
    public int ObjectiveAmount => objectiveAmount;
    public string MissionDescription => missionDescription;
    public MissionState MissionState => missionState;


    //PRIVATE VARIABLES
    private string missionId;
    private string missionName;
    private int levelRequirement;
    private string finishedMissionId;
    private string eventId;
    private string objectName;
    private int objectiveAmount;
    private string missionDescription;
    private MissionState missionState;

    private MissionManager missionManager;


    //INITIALIZER
    public void InitializeMission(string id, string name, int levelReq, string finishedMissionId, string eventReq, string objName, int objAmount, string description)
    {
        SetMissionState(MissionState.Locked);
        SetMissionId(id);
        SetName(name);
        SetLevelRequirement(levelReq);
        SetFinishedMissionsRequirementId(finishedMissionId);
        SetEventId(eventReq);
        //handle mission types suppose to be here
        SetObjectName(objName);
        SetObjectiveAmount(objAmount);
        SetDescription(description);

        missionManager = GetComponentInParent<MissionManager>();
    }

    //MONO BEHAVIOUR
    private void Start()
    {
        PlayerLevelUpCheck(0); //FIND A WAY TO START THE MISSION AUTOMATION
        //IngameEvents.OnPlayerLevelUp += PlayerLevelUpCheck;
        MissionEvents.OnMissionFinished += FinishMissionCheck;
    }

    private void OnDestroy()
    {
        //IngameEvents.OnPlayerLevelUp -= PlayerLevelUpCheck;
        MissionEvents.OnMissionFinished -= FinishMissionCheck;

    }

    //MISSION FUNCTIONS
    public void UnlockMission()
    {
        if (missionState == MissionState.Finished)
            return;

        missionState = MissionState.Ready;

        //For now unlocked mission is automatically started
        StartMission();
        Debug.Log("UnlockedMission " + missionId);
    }

    /// <summary>
    /// Start Mission is triggered by 'Mission Point'
    /// </summary>
    public void StartMission()
    {
        //create the instance of the missionActivity based on missionType
        //for now, only int mission type available

        GameObject activity = new GameObject(ObjectName);
        activity.transform.parent = this.transform;
        
        activity.AddComponent<MissionActivityInt>().InitializeActivity(EventId, ObjectName, ObjectiveAmount);
        
        missionState = MissionState.Active;
        missionManager.MissionStart(MissionId);
    }

    public void FinishMission()
    {
        //Distribute reward
        Debug.Log("Finished mission: " + MissionId);
        missionState = MissionState.Finished;
        missionManager.MissionFinish(MissionId);

    }

    //check if mission is unlockable by FinishMission() of other mission. Doesn't check for player level at the moment
    private void FinishMissionCheck(string id)
    {
        GameObject finishedMission = missionManager.GetMissionById(id);
        
        if(finishedMission.GetComponent<Mission>().MissionId == this.finishedMissionId)
        {
            UnlockMission();
        }
    }

    //check if mission is unlockable by level up
    //Might not need this in automation
    private void PlayerLevelUpCheck(int level)
    {
        //REFACTOR THIS
        if (levelRequirement >= level)
        {             
            if (finishedMissionId == "none")
            {
                UnlockMission();
                return;
            }

            GameObject missionToCheck = missionManager.GetMissionById(finishedMissionId);
            if (missionManager.FinishedMissions.Contains(missionToCheck))
            {
                UnlockMission();
                return;
            }
        }
    }


    //PUBLIC SETTER
    public void SetMissionState(MissionState state) => missionState = state;
    public void SetMissionId(string missionId) => this.missionId = missionId;
    public void SetName(string nameToSet) => missionName = nameToSet;
    public void SetLevelRequirement(int level) => levelRequirement = level;
    public void SetFinishedMissionsRequirementId(string missionId) => finishedMissionId = missionId;
    public void SetEventId(string eventIdToSet) => eventId = eventIdToSet;
    public void SetObjectName(string objectNameToSet) => objectName = objectNameToSet;
    public void SetObjectiveAmount(int amountToSet) => objectiveAmount = amountToSet;
    public void SetDescription(string descriptionToSet) => missionDescription = descriptionToSet;
}

public enum MissionState
{
    Locked, //mission doesn't interact with the main system
    Ready, //mission can be initiated from the 'mission point'
    Active, //mission is started, listening to progress on the main system
    Finished //similar to Locked but marked finish and can be used for checking requirement
}
