using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MissionSequence : MonoBehaviour
{
    private bool isFinished = false;

    private string missionId;

    public void InitializeMissionSequence(string id)
    {
        this.missionId = id;
    }

    protected void FinishMissionSequence()
    {
        if (isFinished) return;

        isFinished = true;

        EventManager.Instance.missionEvents.AdvanceMission(missionId);
        Debug.Log(this.gameObject.name + " sequence is Finished!");

        Destroy(this.gameObject);
    }
}
