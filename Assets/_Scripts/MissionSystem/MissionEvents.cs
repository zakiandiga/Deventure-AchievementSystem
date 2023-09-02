using System;

public class MissionEvents
{
    public event Action<string> OnMissionStarted;
    public event Action<string> OnMissionAdvanced;
    public event Action<string> OnMissionFinished;
    public event Action<Mission> OnMissionStateChanged;

    public void StartMission(string id)
    {
        OnMissionStarted?.Invoke(id);
    }

    public void AdvanceMission(string id)
    {
        OnMissionAdvanced?.Invoke(id);
    }

    public void FinishMission(string id)
    {
        OnMissionFinished?.Invoke(id);
    }

    public void MissionStateChange(Mission mission)
    {
        OnMissionStateChanged?.Invoke(mission);
    }
}
