using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public InGameEvents InGameEvents { get; private set; }
    public MissionEvents MissionEvents { get; private set; }
    public UIEvents UIEvents { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        InGameEvents = new InGameEvents();
        MissionEvents = new MissionEvents();
        UIEvents = new UIEvents();
    }
}
