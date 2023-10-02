using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Mission02 : MonoBehaviour, IComparer< MissionField >
{
    public string Id { get; private set; }
    public string Unlocked { get; private set; }
    public string Description { get; private set; }
    public List<MissionAction> MissionActions => missionActions;

    [SerializeField]private List<MissionAction> missionActions = new List<MissionAction>();
    
    private List< MissionField > fields = new List< MissionField >();

    public void InitializeMission(string id, string unlocked, string description)
    {
        this.Id = id;
        this.Unlocked = unlocked;
        this.Description = description;
    }
    
    public int Compare(MissionField lhs, MissionField rhs)
    {
        return string.Compare( lhs.Name, rhs.Name );
    }
    
    public MissionField GetField(string nom)
    {
        MissionField field = new MissionField( nom, "" );
        int index = fields.BinarySearch( 0, fields.Count, field, this );
        
        if (index < 0)
        {
            fields.Insert( ~index, field );
            return field;
        }
        return fields[ index ];
    }
    
    public bool Evaluate()
    {
        return true; // for now -- TODO: write out the full process of Mission Evaluation here
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