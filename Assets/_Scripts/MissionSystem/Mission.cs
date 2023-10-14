using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mission : MonoBehaviour, IComparer< MissionField >
{
    //PUBLIC PROPERTIES
    public string Id
    {
        get { return GetField( "id" ).AsString(); }
        set { GetField( "id" ).SetString( value ); }
    }
    
    public int Unlocked
    {
        get { return GetField( "unlocked" ).AsNumber(); }
        set { GetField( "unlocked" ).SetString( "" + value ); }
    }
    
    public string Description
    {
        get { return GetField( "description" ).AsString(); }
        set { GetField( "description" ).SetString( value ); }
    }
    
    public MissionState MissionState => missionState;
    public List<MissionAction> MissionActions => missionActions;


    //PRIVATE VARIABLES    
    [SerializeField] private List<MissionAction> missionActions = new List<MissionAction>();
    private List<MissionField> missionFields = new List<MissionField>();

    private MissionState missionState = MissionState.Locked;

    //private MissionManager missionManager;

    public void InitializeMission(string id, string unlocked, string description)
    {        
        Id = id;
        
        int unlocked_number = int.Parse( unlocked );
        Unlocked = unlocked_number;
        
        Description = description;
        
        this.missionState = unlocked_number != 0 ? MissionState.Ready : MissionState.Locked;
    }

    public void AddMissionAction(string lhs, string op, string rhs)
    {
        MissionAction action = new MissionAction( this, missionActions.Count + 1, lhs, op, rhs );
        missionActions.Add( action );
    }

    public int Compare(MissionField lhs, MissionField rhs)
    {
        return string.Compare( lhs.Name, rhs.Name );
    }

    public MissionField GetField(string nom)
    {
        MissionField field = new MissionField( nom, "" );
        int index = missionFields.BinarySearch( 0, missionFields.Count, field, this );

        if (index < 0)
        {
            missionFields.Insert( ~index, field );
            return field;
        }
        
        return missionFields[ index ];
    }

    public bool Evaluate()
    {
        Logger.UIMessage(Id + " description: " + Description);

        int i = 0;
        while (i != missionActions.Count)
        {
            MissionAction action =  missionActions[i];
            ++i;
            
            string output = Id + " MissionAction" + i + ": " + action.LHS.AsString() + " " + action.Op + " " + action.RHS.AsString();
            Logger.UIMessage(output);
            
            if (action.Evaluate() == false) return false;
        }
        
        return true;
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