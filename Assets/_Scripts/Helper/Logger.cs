using UnityEngine;

public static class Logger
{
    public static void UIMessage(string messages)
    {
#if UNITY_EDITOR
        Debug.Log(messages);
#endif

        EventManager.Instance.UIEvents.LogTextDisplay(messages);
    }

    public static void UIMessage(string messages, Color color)
    {
        
    }
}
