using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[System.Serializable]
public class MissionAction
{
    public MissionField LHS;
    
    private MissionField OpField;
    public string Op
    {
        get { return OpField.AsString(); }
        set { OpField.SetString( value ); }
    }
    
    public MissionField RHS;

    public MissionAction(Mission owner, int index, string lhs, string op, string rhs)
    {
        LHS = owner.GetField( "lhs" + index );
        LHS.SetString( lhs );
        
        OpField = owner.GetField( "op" + index );
        Op = op;
        
        RHS = owner.GetField( "rhs" + index );
        RHS.SetString( rhs );
    }

    public bool Evaluate()
    {
        return true;
    }
}

[System.Serializable]
public class MissionField
{
    public string Name { get; private set; }
    private string Text;
    
    public MissionField(string nom, string val)
    {
        Name = nom;
        Text = val;
    }
    
    public string AsString()
    {
        return Text;
    }
    
    public int AsNumber()
    {
        if (Text == "") return 0;
        
        try
        {
            return Int32.Parse( Text );
        }
        catch (FormatException)
        {
            return 1;
        }
    }
    
    public bool SetString(string val)
    {
        Text = val;
        return true;
    }
}

public class Datum
{
    private string Location { get; set; }
    private int Value { get; set; }
    private Mission Target { get; set; }
    private MissionField Field { get; set; } 
    
    public delegate string ReadString();
    public ReadString AsString;
    
    public delegate int ReadInt();
    public ReadInt AsNumber;
    
    public delegate bool WriteString(string val);
    public WriteString SetString;
    
    public string ReadLocation()
    {
        return Location;
    }
    
    public int ReadValue()
    {
        return Value;
    }
    
    public bool WriteNothing(string val)
    {
        return false;
    }
    
    public bool WriteLocation(string val)
    {
        Location = val;
        return true;
    }
    
    public string ReadMissionString()
    {
        bool outcome = Target.Evaluate();
        if (outcome) return Target.GetField( "description" ).AsString();
        return "";
    }
    
    public int ReadMissionNumber()
    {
        bool outcome = Target.Evaluate();
        if (outcome) return 1;
        return 0;
    }
    
    public Datum(Mission owner, string operand)
    {
        Location = operand;
        AsString = new ReadString( ReadLocation );
        AsNumber = new ReadInt( ReadValue );
        SetString = new WriteString( WriteNothing );
        
        try
        {
            Value = Int32.Parse( operand );
        }
        catch (FormatException)
        {
            var values = Regex.Split( operand, "." );
            if (values.Length == 0 || values.Length > 2 || (values.Length == 2 && values[ 1 ] == ""))
            {
                Value = 0;
            }
            else
            {
                try
                {
                    Target = GameObject.Find( values[ 0 ] ).GetComponent< Mission >();
                }
                catch (Exception)
                {
                    if (values.Length == 2)
                    {
                        GameObject missionObject = new GameObject( values[ 0 ] );
                        Target = missionObject.AddComponent< Mission >();
                        Target.InitializeMission( values[ 0 ], "0", "" );
                    }
                    else
                    {
                        return; // It's just a plain string. Nothing more.
                    }
                }
                
                if (values.Length == 2)
                {
                    Field = Target.GetField( values[ 1 ] );
                    AsString = new ReadString( Field.AsString );
                    AsNumber = new ReadInt( Field.AsNumber );
                    SetString = new WriteString( Field.SetString );
                }
                else
                {
                    AsString = new ReadString( ReadMissionString );
                    AsNumber = new ReadInt( ReadMissionNumber );
                }
            }
        }
    }
}