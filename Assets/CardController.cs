using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    private HandManager handManager;
    public void BindHandManager(HandManager handManager)
    {
        this.handManager = handManager;
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        handManager.HoverCard(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        handManager.SelectCard(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handManager.ReleaseCard(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        handManager.ExitCard(this);
    }

}
