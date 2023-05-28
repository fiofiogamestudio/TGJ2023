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

    public List<EventSpritePair> CardSpriteMap = new List<EventSpritePair>();

    public Sprite GetEventSprite(string name)
    {
        foreach (var pair in CardSpriteMap)
        {
            if (pair.name == name) return pair.sprite;
        }
        return null;
    }
}
