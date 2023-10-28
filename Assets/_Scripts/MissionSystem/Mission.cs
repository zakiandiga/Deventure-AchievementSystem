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

    public List<MissionAction> MissionActions => missionActions;

    //PRIVATE VARIABLES    
    [SerializeField] private List<MissionAction> missionActions = new List<MissionAction>();
    private List<MissionField> missionFields = new List<MissionField>();

    public void InitializeMission(string id, string unlocked, string description)
    {        
        Id = id;
        
        GetField( "unlocked" ).SetString( unlocked );
        
        Description = description;        
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
        int i = 0;
        while (i != missionActions.Count)
        {
            MissionAction action =  missionActions[i];
            ++i;
            
            string output = Id + " MissionAction" + i + ": " + action.LHS.AsString() + " " + action.Op + " " + action.RHS.AsString();
            Debug.Log( output );
            
            if (action.Evaluate() == false) return false;
        }
        
        if (Description != "" && Description != "NULL")
        {
            Logger.UIMessage(Description);
            Debug.Log(Id + " | description: " + Description);
        }
        
        return true;
    }
}