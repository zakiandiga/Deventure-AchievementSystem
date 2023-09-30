using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Datum
{
    private string Location { get; set; }
    private int Value { get; set; }
    
    public delegate string ReadString();
    public delegate int ReadInt();
    public delegate bool WriteString(string val);
    
    public string ReadLocation()
    {
        return Location;
    }
    
    public int ReadValue()
    {
        return Value;
    }
    
    public bool WriteNothing()
    {
        return false;
    }
    
    public void WriteLocation(string val)
    {
        Location = val;
    }
    
    public Datum(string operand)
    {
        Location = operand;
        
        try
        {
            Value = Int32.Parse( operand );
            ReadString = ReadLocation;
            ReadInt = ReadValue;
            WriteString = WriteNothing;
        }
        catch (FormatException)
        {
            //Console.WriteLine($"Unable to parse '{input}'");
        }
    }
    
/*
    int
    Mission
    Globals.field
    Mission.field
 */  
}

[System.Serializable]
public class MissionAction
{
    public string Lhs { get; set; }
    public string Op { get; set; }
    public string Rhs { get; set; }
    public bool ActionFinished { get; set; } //not sure if we need this

    public MissionAction(string lhs, string op, string rhs)
    {
        this.Lhs = lhs;
        this.Op = op;
        this.Rhs = rhs;
    }

    public void InitiateAction()
    {
        //check if variable exist

        //Evaluate()
        //Assign
        //CheckCondition
    
        ActionFinished = true;
    }
}
