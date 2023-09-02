using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTestInt : MissionSequence
{
    private int missionInt = 0;
    private int missionIntRequirement = 5;

    private void OnEnable()
    {
        TesterInput.OnTestKeyInt += IntRecieved;        
    }

    private void OnDisable()
    {
        TesterInput.OnTestKeyInt -= IntRecieved;
    }

    private void IntRecieved(int value)
    {
        missionInt += value;

        Debug.Log("Mission int value added!");

        if(missionInt >= missionIntRequirement)
        {
            FinishMissionSequence();
        }
    }
}
