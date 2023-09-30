using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
