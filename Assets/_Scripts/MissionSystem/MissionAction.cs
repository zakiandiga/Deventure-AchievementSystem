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
        Datum lhs = new Datum( LHS.AsString() );
        Datum rhs = new Datum( RHS.AsString() );
        int lvalue = 0;
        int rvalue = 0;
        
        switch (Op)
        {
            case "Subtract -=":
                lhs.SetString( "" + (lhs.AsNumber() - rhs.AsNumber()) );
                return true;
                
            case "Assign =":
                lhs.SetString( rhs.AsString() );
                return true;
                
            case "Add +=":
                lhs.SetString( "" + (lhs.AsNumber() + rhs.AsNumber()) );
                return true;
                
            case "Multiply *=":
                lhs.SetString( "" + (lhs.AsNumber() * rhs.AsNumber()) );
                return true;
                
            case "Divide /=":
                rvalue = rhs.AsNumber();
                if (rvalue == 0)
                {
                    Debug.LogError( "Division by 0." );
                    return false;
                }
                
                lhs.SetString( "" + (lhs.AsNumber() / rvalue) );
                return true;
                
            case "Modulo %=":
                rvalue = rhs.AsNumber();
                if (rvalue == 0)
                {
                    Debug.LogError( "Division by 0." );
                    return false;
                }
                
                lhs.SetString( "" + (lhs.AsNumber() % rvalue) );
                return true;
                
            case "IsEqual ==":
                return lhs.AsString() == rhs.AsString();
                
            case "IsLesser <":
                lvalue = lhs.AsNumber();
                rvalue = rhs.AsNumber();
                if (lvalue == rvalue) return string.Compare( lhs.AsString(), rhs.AsString() ) < 0;
                return lvalue < rvalue;
                
            case "IsGreater >":
                lvalue = lhs.AsNumber();
                rvalue = rhs.AsNumber();
                if (lvalue == rvalue) return string.Compare( lhs.AsString(), rhs.AsString() ) > 0;
                return lvalue > rvalue;
                
            case "Not !":
                rvalue = lhs.AsNumber();
                bool notted = (rvalue == 0);
                lhs.SetString( notted ? "1" : "0" );
                return notted;
                
            case "RNGFrom":
                var random = new System.Random();
                int outcome = random.Next( new Datum( RHS.AsString() + ".lo" ).AsNumber(), new Datum( RHS.AsString() + ".hi" ).AsNumber() + 1 );
                lhs.SetString( "" + outcome );
                return true;
                
            case "Evaluate()":
                return lhs.AsNumber() != 0;
                
            default:
                Debug.LogError( "Unrecognized op." );
                return false;
        }
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
        if (Text == "" || Text == "NULL") return 0;
        
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

[System.Serializable]
public class Datum
{
    private string Location { get; set; }
    private int Value { get; set; }
    private Mission Target { get; set; } //The mission this datum pointing towards
    private MissionField Field { get; set; } //Field that the target possess (ID, Unlocked, Lhs, etc)
    
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
    
    public Datum(string operand)
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
            var values = Regex.Split( operand, "\\." );
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
