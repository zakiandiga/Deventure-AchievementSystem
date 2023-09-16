using System;
using UnityEngine;

public class MissionEvents
{
    public static event Action<string> OnMissionStarted;
    public static event Action<string> OnMissionFinished;

    public void StartMission(string id)
    {
        OnMissionStarted?.Invoke(id);
    }

    public void FinishMission(string id)
    {
        OnMissionFinished?.Invoke(id);
    }

}
