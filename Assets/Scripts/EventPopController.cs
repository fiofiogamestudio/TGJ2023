using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventPopController : MonoBehaviour
{
    public Image EventImage;

    public Text EventTitle;
    public Text EventDescription;

    [ReadOnly]
    public EventType eventType;
    [ReadOnly]
    public string eventName;
    [ReadOnly]
    public string eventDescription;
    [ReadOnly]
    public string[] eventEffects;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.gameObject.SetActive(false);
            // call event effects
            CardEffectManager.instance.ExecuteEffects(eventEffects);
        }
    }

    public void InitData(EventData data)
    {
        eventType = data.eventType;
        eventName = data.eventName;
        eventDescription = data.eventDescription;
        eventEffects = data.eventEffects;

        // init view
        EventImage.sprite = EventSpriteHolder.instance.GetEventSprite(eventName);
        EventTitle.text = eventName;
        EventDescription.text = eventDescription;
    }

    private string colorToHex(Color color)
    {
        Color32 color32 = color;
        string hexColor = "#" + System.BitConverter.ToString(new byte[] { color32.r, color32.g, color32.b, color32.a }).Replace("-", "");
        return hexColor;

    }
}
