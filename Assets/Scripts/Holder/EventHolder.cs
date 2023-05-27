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
    public EventData[] NormalDatas;
    [ReadOnly]
    public EventData[] RandomDatas;

    void Awake()
    {
        loadEvents();
    }

    void loadEvents()
    {
        EventWrapper eventWrapper = DataLoader.LoadJson<EventWrapper>("Events");
        GoodEvents = eventWrapper.goodEvents;
        BadEvents = eventWrapper.badEvents;
        NormalDatas = eventWrapper.normalEvents;
        RandomDatas = eventWrapper.randomEvents;
    }
}
