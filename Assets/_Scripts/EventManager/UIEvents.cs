using System;
using UnityEngine;

public class UIEvents
{
    public static event Action<string> OnLogTextSent;

    public void LogTextDisplay(string message)
    {
        OnLogTextSent?.Invoke(message);
    }
}
