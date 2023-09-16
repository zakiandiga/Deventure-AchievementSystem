using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public IngameEvents ingameEvents;
    public MissionEvents missionEvents;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        ingameEvents = new IngameEvents();
        missionEvents = new MissionEvents();

    }
}
