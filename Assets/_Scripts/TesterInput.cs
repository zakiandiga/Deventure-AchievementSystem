using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TesterInput : MonoBehaviour
{
    public string objectName = "wood";

    private KeyCode Q = KeyCode.Q; //int test
    private KeyCode W = KeyCode.W; //bool test
    private KeyCode E = KeyCode.E; //start quest test
    private KeyCode R = KeyCode.R;
    private KeyCode T = KeyCode.T;

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
            DebugLevelUp();
        }

        if(Input.GetKeyDown(E))
        {
            TestStartQuest();
        }
   
        if(Input.GetKeyDown(R))
        {
            DebugObtainItem();
        }

        if(Input.GetKeyDown(T))
        {
            DebugKillMonster();
        }

    }

    private void TestInt()
    {
        OnTestKeyInt?.Invoke(1);
    }

    private void DebugLevelUp()
    {
        OnTestKeyBool?.Invoke(true);
    }

    private void TestStartQuest()
    {
        EventManager.Instance.missionEvents.StartMission("MissionIntTest");
    }

    private void DebugObtainItem()
    {
        EventManager.Instance.ingameEvents.ObtainItem(objectName, 1);
    }

    private void DebugKillMonster()
    {
        EventManager.Instance.ingameEvents.KillEnemy(objectName);
    }


}
