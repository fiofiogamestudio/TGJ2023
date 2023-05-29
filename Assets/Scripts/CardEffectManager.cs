using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class CardEffectManager : MonoBehaviour
{
    public static CardEffectManager instance;
    void Awake() { if (instance == null) instance = this; }


    void Start()
    {
        // Test
        // executeEffect("food, 10");
        // executeEffect("hurt");
        // executeEffect("hurt, 5");
        // executeEffect("new_event");
        // executeEffect("new_event, bad");
        // executeEffect("new_event, good");
        // executeEffect("people, 1");
        // executeEffect("buff");
        // executeEffect("plan");
        // executeEffect("hurt_or_die");
    }

    public void ExecuteEffects(string[] effects)
    {
        foreach (var effect in effects)
        {
            executeEffect(effect);
        }
    }

    private List<string> temp_args = new List<string>();
    private void executeEffect(string effect)
    {
        Debug.Log("execute effect: " + effect);

        temp_args.Clear();
        string[] strs = effect.Split(",");
        string to_call = strs[0];
        for (int i = 1; i < strs.Length; i++)
        {
            temp_args.Add(strs[i].Trim()); // remove space
        }
        // custom invoke using reflection
        MethodInfo methodInfo = GetType().GetMethod(to_call, BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo != null)
        {
            methodInfo.Invoke(this, null);
        }
        else
        {
            Debug.LogWarning("method " + to_call + " not found!");
        }
    }


    private void food()
    {
        int count = System.Convert.ToInt32(temp_args[0]);
        // Debug.Log("food " + count);
        GameManager.instance.AddFood(count);
    }

    private void hurt()
    {
        if (temp_args.Count == 0)
        {
            // Debug.Log("hurt " + "100%");
            HandManager.instance.GenerateCard("受伤");
        }
        else
        {
            int prob = System.Convert.ToInt32(temp_args[0]);
            // Debug.Log("hurt " + prob);
            float f_prob = (float)prob / 10.0f;
            if (Random.Range(0.0f, 1.0f) < f_prob) HandManager.instance.GenerateCard("受伤");
        }
    }

    private void new_event()
    {
        if (temp_args.Count == 0)
        {
            // Debug.Log("new event");
            GameManager.instance.AddEvent(EventType.RandomEvent);
        }
        else
        {
            string type = temp_args[0];
            if (type == "bad")
            {
                // Debug.Log("new event bad");
                GameManager.instance.AddEvent(EventType.BadEvent);
            }
            else if (type == "good")
            {
                // Debug.Log("new event good");
                GameManager.instance.AddEvent(EventType.GoodEvent);
            }
        }
    }

    private void people()
    {
        int count = System.Convert.ToInt32(temp_args[0]);
        // Debug.Log("people " + count);
        GameManager.instance.AddPeople(1);
    }

    private void people_die()
    {
        // Debug.Log("people die!");
    }

    private void buff()
    {
        Debug.Log("buff");
    }

    private void plan()
    {
        // Debug.Log("plan");
        float f_p = Random.Range(0.0f, 3.0f);
        if (f_p < 1.0f)
        {
            HandManager.instance.GenerateCard("建造家园");
        }
        else if (f_p < 2.0f)
        {
            HandManager.instance.GenerateCard("探索世界");
        }
        else
        {
            HandManager.instance.GenerateCard("祭祀神祗");
        }
    }

    private void hurt_or_die()
    {
        // Debug.Log("hurt_or_die");
        if (Random.Range(0.0f, 1.0f) < 0.5f)
        {
            HandManager.instance.GenerateCard("受伤");
        }
        else
        {
            HandManager.instance.GenerateCard("死亡");
        }
    }

    private void add_card()
    {
        string cardName = temp_args[0];
        HandManager.instance.GenerateCard(cardName);
    }
}
