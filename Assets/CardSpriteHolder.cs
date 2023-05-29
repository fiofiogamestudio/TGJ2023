using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpriteHolder : MonoBehaviour
{
    [System.Serializable]
    public class CardSpritePair
    {
        public string name;
        public Sprite sprite;
    }

    public List<CardSpritePair> CardSpriteMap = new List<CardSpritePair>();

    public static CardSpriteHolder instance;
    void Awake()
    {
        if (instance == null) instance = this;
    }

    public Sprite GetCardSprite(string name)
    {
        foreach (var pair in CardSpriteMap)
        {
            if (pair.name == name) return pair.sprite;
        }
        return null;
    }
}
