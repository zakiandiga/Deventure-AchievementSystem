using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int Gold => _gold;
    public int Experience => _experience;

    private int _gold = 0;
    private int _experience = 0;

    public void ModifyGold(int value)
    {
        _gold = Mathf.Max(0, _gold + value);         
    }

    public void ModifyExperience(int value)
    {
        _experience += value;
    }
}
