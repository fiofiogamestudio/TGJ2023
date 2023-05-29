using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSpriteHolder : MonoBehaviour
{
    [System.Serializable]
    public class EventSpritePair
    {
        public string name;
        public Sprite sprite;
    }

    public List<EventSpritePair> EventSpriteMap = new List<EventSpritePair>();

    public static EventSpriteHolder instance;
    void Awake() { if (instance == null) instance = this; }

    public Sprite GetEventSprite(string name)
    {
        foreach (var pair in EventSpriteMap)
        {
            if (pair.name == name) return pair.sprite;
        }
        return null;
    }
}
