using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    public GameObject Border;

    private float width = 2.1f; // temp 210 * 0.01
    private float height = 2.8f; // temp 280 * 0.01

    public bool checkInGrid(Vector3 worldPos)
    {
        Vector3 dir = worldPos - transform.position;
        return (Mathf.Abs(dir.x) < width * 0.5f && Mathf.Abs(dir.z) < height * 0.5f);
    }



    [ReadOnly]
    public bool binding = false;
    public CardController bindedCard;

    public void BindCard(CardController card)
    {
        this.binding = true;
        this.bindedCard = card;
    }

    public void UnBind()
    {
        this.binding = false;
        this.bindedCard = null;
    }

    [ReadOnly]
    public bool hovering = false;
    public void Hover()
    {
        hovering = true;
        // this.GetComponent<Image>().color = Color.red;
        Border.SetActive(true);
    }

    public void Leave()
    {
        if (hovering)
        {
            hovering = false;
            // this.GetComponent<Image>().color = Color.white;
            Border.SetActive(false);
        }
    }

}
