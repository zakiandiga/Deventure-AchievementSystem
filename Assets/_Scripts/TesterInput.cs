using System;
using System.Collections.Generic;
using UnityEngine;

public class TesterInput : MonoBehaviour
{
    private KeyCode Q = KeyCode.Q; //int test
    private KeyCode W = KeyCode.W; //bool test
    private KeyCode E = KeyCode.E; //start quest test
    private KeyCode R = KeyCode.R;

    public static event Action<int> OnTestKeyInt;
    public static event Action<bool> OnTestKeyBool;

    void Update()
    {
        if (Input.GetKeyDown(Q))
        {
            TestInt();
        }

        if(Input.GetKeyDown(W))
        {
            TestBool();
        }

        if(Input.GetKeyDown(E))
        {
            TestStartQuest();
        }
   

    }

    private void TestInt()
    {
        OnTestKeyInt?.Invoke(1);
    }

    private void TestBool()
    {
        OnTestKeyBool?.Invoke(true);
    }

    private void TestStartQuest()
    {
        EventManager.Instance.missionEvents.StartMission("MissionIntTest");
    }


}
