using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MissionActivity : MonoBehaviour
{
    private string missionId;

    public void StartActivity(string id)
    {
        missionId = id;
    }

    protected void FinishActivity()
    {
        Debug.Log("Finished activity");
        Destroy(gameObject);
    }
}
