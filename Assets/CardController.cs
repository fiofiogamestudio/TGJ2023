using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    private HandManager handManager;

    [ReadOnly]
    public bool binding;
    [ReadOnly]
    public GridController bindedGrid;

    // Bing Manager
    public void BindHandManager(HandManager handManager)
    {
        this.handManager = handManager;
    }

    // Bind Grid
    public void BindGrid(GridController grid)
    {
        this.binding = true;
        this.bindedGrid = grid;
    }

    public void UnBind()
    {
        this.binding = false;
        this.bindedGrid = null;
    }

    void FixedUpdate()
    {
        // lerp to binded grid
        if (binding)
        {
            if (Vector3.Distance(transform.position, bindedGrid.transform.position) > 0.01f)
            {
                transform.position = Vector3.Lerp(
                    transform.position,
                    bindedGrid.transform.position,
                    0.2f);

            }
            else
            {
                transform.position = bindedGrid.transform.position;
            }
        }
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
