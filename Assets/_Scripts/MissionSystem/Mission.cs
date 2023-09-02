using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Mission
{
    //MISSION VARIABLES
    private MissionInfo missionInfo;
    private MissionState missionState; //CURRENTLY NOT UTILIZED YET
    private int currentMissionSequenceIndex;
    private int totalMissionSequence;

    //MISSION CONSTRUCTOR
    public Mission(MissionInfo missionInfo)
    {
        this.missionInfo = missionInfo;
        this.missionState = MissionState.Locked;
        this.currentMissionSequenceIndex = 0;
        this.totalMissionSequence = missionInfo.missionSequences.Length;
    }

    //MISSION FUNCTIONS
    public void AdvanceMissionSequence()
    {
        currentMissionSequenceIndex++;


    }

    private GameObject GetCurrentMissionSequencePrefab()
    {
        GameObject missionSequencePrefab = GetMissionInfo().missionSequences[currentMissionSequenceIndex];

        if (!NextSequenceAvailable())
        {
            Debug.LogWarning("No more mission sequence available!");
        }

        return missionSequencePrefab;
    }

    public void InstantiateCurrentMissionSequence(Transform parent)
    {
        GameObject missionSequencePrefab = GetMissionInfo().missionSequences[currentMissionSequenceIndex];

        if(missionSequencePrefab == null)
        {
            Debug.LogError("No more mission sequence available!");
            return;
        }

        //consider pooling for this
        MissionSequence missionSequence = Object.Instantiate<GameObject>(missionSequencePrefab, parent).GetComponent<MissionSequence>();
        missionSequence.InitializeMissionSequence(GetMissionInfo().id);
    }

    //MISSION PUBLIC GETTER
    public MissionInfo GetMissionInfo() => missionInfo;

    public MissionState GetMissionState() => missionState;

    public bool NextSequenceAvailable()
    {
        return currentMissionSequenceIndex < missionInfo.missionSequences.Length;
    }

}

public enum MissionState
{
    Locked,
    Unlocked,
    Finished
}