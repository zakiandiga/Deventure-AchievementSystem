using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public MissionEvents missionEvents;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        missionEvents = new MissionEvents();

    }
}
