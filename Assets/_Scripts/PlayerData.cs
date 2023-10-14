using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int Gold => gold;
    public int Experience => experience;
    public int Level => level;

    private int gold = 0;
    private int experience = 0;
    private int level = 1;

    //MAIN FUNCTIONS
    public void LevelUp()
    {
        level += 1;
        EventManager.Instance.InGameEvents.PlayerLevelUp(level);
    } 

    public void AddGold(int value) => gold = Mathf.Max(0, gold + value);         

    public void AddExperience(int value) => experience += value;


    //DEBUG FUNCTIONS
    public void SetGold(int value) => gold = value;

}
