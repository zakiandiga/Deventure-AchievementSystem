using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Mission02 : MonoBehaviour
{
    public string Id { get; private set; }
    public string Unlocked { get; private set; }
    public string Description { get; private set; }
    public List<MissionAction> MissionActions => missionActions;

    [SerializeField]private List<MissionAction> missionActions = new List<MissionAction>();


    public void InitializeMission(string id, string unlocked, string description)
    {
        this.Id = id;
        this.Unlocked = unlocked;
        this.Description = description;
    }

    public void AddMissionAction(string lhs, string op, string rhs)
    {
        MissionAction action = new MissionAction(lhs, op, rhs);
        missionActions.Add(action);
    }

    public void PrintMission()
    {
        Debug.Log("Mission ID: " + Id +
            " || Mission Unlocked: " + Unlocked +
            " || Mission Description: " + Description);
        for (int i = 0; i < missionActions.Count; i++)
        {
            string lhs = missionActions[i].Lhs;
            string op = missionActions[i].Op;
            string rhs = missionActions[i].Rhs;

            Debug.Log(Id + "MissionAction" + (i + 1) + ": " + lhs + " " + op + " " + rhs);            
        }
    }

}

[System.Serializable]
public class MissionAction
{
    public string Lhs { get; set; }
    public string Op { get; set; }
    public string Rhs { get; set; }

    public MissionAction(string lhs, string op, string rhs)
    {
        this.Lhs = lhs;
        this.Op = op;
        this.Rhs = rhs;
    }
}