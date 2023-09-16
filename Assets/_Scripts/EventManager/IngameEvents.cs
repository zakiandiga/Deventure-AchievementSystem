using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameEvents
{
    public static event Action<string, int> OnObtainItem;
    public static event Action<string, int> OnKillEnemy;

    public static event Action<int> OnPlayerLevelUp;

    public void ObtainItem(string itemName, int amount)
    {
        OnObtainItem?.Invoke(itemName, amount);
    }

    public void KillEnemy(string enemyName)
    {
        OnKillEnemy?.Invoke(enemyName, 1);
    }

    public void PlayerLevelUp(int nextLevel)
    {
        OnPlayerLevelUp?.Invoke(nextLevel);
    }
}
