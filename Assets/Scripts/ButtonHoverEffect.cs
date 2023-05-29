using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    private Color InitColor;
    void Awake()
    {
        InitColor = this.GetComponent<Image>().color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.GetComponent<Image>().color = Color.yellow;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.GetComponent<Image>().color = InitColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.GetComponent<Image>().color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.GetComponent<Image>().color = InitColor;
    }
}
