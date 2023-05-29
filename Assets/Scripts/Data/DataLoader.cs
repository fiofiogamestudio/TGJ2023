using System.Collections;
using UnityEngine;

public class DataLoader
{
    public static T LoadJson<T>(string path)
    {
        TextAsset jsonText = Resources.Load<TextAsset>(path);
        T temp = JsonUtility.FromJson<T>(jsonText.text);
        return temp;
    }
}

public class DataDebuger
{
    public static void DebugEvent(EventWrapper eventWrapper)
    {
        Debug.Log("Good Events");
        foreach (var goodEvent in eventWrapper.goodEvents)
        {
            Debug.Log("\t " + goodEvent.eventName);
        }

        Debug.Log("Bad Events");
        foreach (var badEvent in eventWrapper.badEvents)
        {
            Debug.Log("\t " + badEvent.eventName);
        }

        Debug.Log("Normal Events");
        foreach (var normalEvent in eventWrapper.normalEvents)
        {
            Debug.Log("\t " + normalEvent.eventName);
        }

    }
}