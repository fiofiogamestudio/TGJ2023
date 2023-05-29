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
            Debug.Log("new event");
        }
        else
        {
            string type = temp_args[0];
            if (type == "bad")
            {
                Debug.Log("new event bad");
            }
            else if (type == "good")
            {
                Debug.Log("new event good");
            }
        }
    }

    private void people()
    {
        int count = System.Convert.ToInt32(temp_args[0]);
        // Debug.Log("people " + count);
        GameManager.instance.AddPeople(1);
    }

    private void buff()
    {
        Debug.Log("buff");
    }

    private void plan()
    {
        Debug.Log("plan");
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
}
