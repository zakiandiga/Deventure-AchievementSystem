using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionInfo", menuName = "ScriptableObjects/Mission", order = 1)]
public class MissionInfo : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    public string displayName = "Mission Name";

    public GameObject[] missionSequences;

    //TODO
    //Requirement Obj
    //Outcome Obj



    private void OnValidate()
    {
#if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}


