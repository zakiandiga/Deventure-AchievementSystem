using System;
using System.Reflection;
using UnityEditor.PackageManager;
using UnityEngine;

public class MissionActivityInt : MissionActivity
{
    private Mission mission;
    private string eventId; //fetch from CSV
    private string targetObject;

    private int requiredAmount = 0; //fetch from CSV
    private int currentAmount = 0;

    //EVENT HANDLER SETUP VARIABLES
    private Type ingameEvent = typeof(IngameEvents);
    private EventInfo eventToSubscribe = null;
    private Action<string, int> eventHandler;

    private void Start()
    {
        eventToSubscribe = ingameEvent.GetEvent(eventId);

        if(eventToSubscribe == null)
        {
            Debug.LogWarning("Error: " + eventId + " event not found!");
            return;
        }

        eventHandler = (Action<string, int>)Delegate.CreateDelegate(typeof(Action<string, int>), this, "UpdateAmount");

        eventToSubscribe.AddEventHandler(null, eventHandler);
    }

    public void InitializeActivity(string eventIdToInitialize, string objectName, int targetAmount)
    {
        mission = GetComponentInParent<Mission>();
        eventId = eventIdToInitialize;
        targetObject = objectName;
        requiredAmount = targetAmount;
    }

    //For now updated through IngameEvents triggered from KeyDown
    private void UpdateAmount(string itemName, int amount)
    {
        if(itemName != targetObject)
        {
            return;
        }

        currentAmount += 1;
        Debug.Log("Updated " + itemName + " amount by " + amount);

        if(currentAmount >= requiredAmount)
        {
            eventToSubscribe.RemoveEventHandler(null, eventHandler);
            mission.FinishMission();
            FinishActivity(); //override the function to suit our need
        }
    }
}
