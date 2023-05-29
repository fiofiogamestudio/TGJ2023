using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHolder : MonoBehaviour
{
    [Header("Events Info")]
    [ReadOnly]
    public EventData[] GoodEvents;
    [ReadOnly]
    public EventData[] BadEvents;
    [ReadOnly]
    public EventData[] NormalEvents;

    public static EventHolder instance;
    void Awake()
    {
        if (instance == null) instance = this;
        loadEvents();
    }

    void loadEvents()
    {
        EventWrapper eventWrapper = DataLoader.LoadJson<EventWrapper>("Events");
        GoodEvents = eventWrapper.goodEvents;
        BadEvents = eventWrapper.badEvents;
        NormalEvents = eventWrapper.normalEvents;
    }

    public EventData GetGoodEvent()
    {
        int index = Random.Range(0, GoodEvents.Length);
        return GoodEvents[index];
    }

    public EventData GetBadEvent()
    {
        int index = Random.Range(0, BadEvents.Length);
        return BadEvents[index];
    }

    public EventData GetNormalEvent()
    {
        int index = Random.Range(0, NormalEvents.Length);
        return NormalEvents[index];
    }

    public EventData GetRandomEvent(GameData data)
    {
        // Test
        // return GetBadEvent();
        int good = data.goodPossibility;
        int bad = data.badPossibility;
        int normal = data.normalPossibility;
        bad += good;
        normal += bad;
        int i = Random.Range(1, normal + 1);
        if (i < good)
        {
            return GetGoodEvent();
        }
        else if (i < bad)
        {
            return GetBadEvent();
        }
        else
        {
            return GetNormalEvent();
        }
    }
}
