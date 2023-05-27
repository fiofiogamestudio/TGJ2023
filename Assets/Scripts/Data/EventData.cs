using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    GoodEvent,
    BadEvent,
    NormalEvent,
    RandomEvent
}

[System.Serializable]
public class EventData
{
    public EventType eventType;
    public string eventName;
    public string eventDescription;
    public string[] eventEffects;
}

[System.Serializable]
public class EventWrapper
{
    public EventData[] goodEvents;
    public EventData[] badEvents;
    public EventData[] normalEvents;
    public EventData[] randomEvents;
}