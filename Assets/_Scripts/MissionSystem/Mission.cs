using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mission : MonoBehaviour, IComparer<MissionField>
{
    //PUBLIC PROPERTIES
    public string Id { get; private set; }
    public bool Unlocked { get; private set; }
    public string Description { get; private set; }
    public MissionState MissionState => missionState;
    public List<MissionAction> MissionActions => missionActions;


    //PRIVATE VARIABLES
    [SerializeField]private List<MissionAction> missionActions = new List<MissionAction>();
   
    private List<MissionField> missionFields = new List<MissionField>();

    private MissionState missionState = MissionState.Locked;

    private MissionManager missionManager;

    public void InitializeMission(string id, string unlocked, string description)
    {
        this.Id = id;
        this.Description = description;
        this.Unlocked = int.Parse(unlocked) > 0;
        this.missionState = int.Parse(unlocked) > 0 ? MissionState.Ready : MissionState.Locked;

        missionManager = GetComponentInParent<MissionManager>();
    }

    public void AddMissionAction(string lhs, string op, string rhs)
    {
        MissionAction action = new MissionAction(lhs, op, rhs);
        missionActions.Add(action);
    }

    public void PrintMission()
    {
        Debug.Log("Mission ID: " + Id + " | Mission Unlocked: " + Unlocked + " | Mission Description: " + Description);
        for (int i = 0; i < missionActions.Count; i++)
        {
            string lhs = missionActions[i].Lhs;
            string op = missionActions[i].Op;
            string rhs = missionActions[i].Rhs;

            Debug.Log(Id + " MissionAction" + (i + 1) + ": " + lhs + " " + op + " " + rhs);            
        }
    }

    public void StartMission()
    {
        if (missionState == MissionState.Active) return;

        missionState = MissionState.Active;

        Debug.Log("Starting mission: " + Id);

        EventManager.Instance.missionEvents.StartMission(Id);
        ExecuteMissionActions();
    }

    private void FinishMission()
    {
        missionState = MissionState.Finished;

        Debug.Log("Finishing mission: " + Id);
        
        missionManager.MissionFinishCleanUp(Id);

        EventManager.Instance.missionEvents.FinishMission(Id);
    }

    public void ExecuteMissionActions()
    {        
        if(missionActions.Count <= 0)
        {
            Debug.Log("Mission " + Id + " doesn't have missionActions!");
            return;
        }
        
        for (int i = 0; i < missionActions.Count; i++)
        {
            missionActions[i].InitiateAction();
            Debug.Log("Mission: " + Id + " | MissionAction " + missionActions[i].Lhs + " initiated");
        }

        FinishMission();
    }

    public int Compare(MissionField lhs, MissionField rhs)
    {
        return string.Compare(lhs.Name, rhs.Name);
    }

    public MissionField GetField(string nom)
    {
        MissionField field = new MissionField(nom, "");
        int index = missionFields.BinarySearch(0, missionFields.Count, field, this);

        if (index < 0)
        {
            missionFields.Insert(~index, field);
            return field;
        }
        return missionFields[index];
    }

    public bool Evaluate()
    {
        return true; // for now -- TODO: write out the full process of Mission Evaluation here
    }
}


[System.Serializable]
public enum MissionState
{
    Locked, //mission doesn't interact with the main system
    Ready, //mission can be initiated from the 'mission point'
    Active, //mission is started, listening to progress on the main system
    Finished //similar to Locked but marked finish and can be used for checking requirement
}