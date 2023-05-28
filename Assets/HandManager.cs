using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public HandLayout TheHandLayout;
    public Transform HandSpawnTransform;
    public Transform HandDestroyTransform;
    public GameObject CardPrefab;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GenerateCard();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            DiscardCard(HandCardList[0]);
        }

        // 检测鼠标是否在Grid上
        checkGrids();

        // 检测鼠标松开事件 如果当前有选择Card 并放在Grid上 就执行Card（Mission）
        checkRelease();
    }

    public GridController hoveredGrid = null;

    private void checkGrids()
    {
        Vector3 planedPos = InputManager.GetMousePosPlaned();
        // Debug.Log(planedPos + " " + GridList[0].transform.position);
        hoveredGrid = null;
        foreach (var grid in GridList)
        {
            bool checkResult = grid.checkInGrid(planedPos);
            if (checkResult && hoveredGrid != grid)
            {
                hoveredGrid = grid;
                // Hover
                grid.Hover();
            }
            else
            {
                grid.Leave();
            }
        }
    }

    private void checkRelease()
    {
        if (!InputManager.GetMouseHolding())
        {
            if (!InputManager.GetMouseRelease())
            {
                // Debug.Log("not handle");
                // 切换程序导致的松开 不处理
                if (selectedCard) ReleaseCard(selectedCard);
            }
            else
            {
                // 手动的松开 处理
                if (selectedCard && hoveredGrid)
                {
                    Debug.Log("handle");
                    PlaceCard(selectedCard, hoveredGrid);
                }
            }

        }


    }

    public void GenerateCard()
    {
        spawnCard(draw: false);
    }

    public void DiscardCard(CardController card)
    {
        destroyCard(card);
    }

    public void PlaceCard(CardController card, GridController grid)
    {
        placeCard(card, grid);
    }

    private void spawnCard(bool draw = false)
    {
        GameObject cardObject = GameObject.Instantiate(CardPrefab);
        // Add to Layout
        cardObject.transform.SetParent(TheHandLayout.transform);
        cardObject.transform.position = HandSpawnTransform.position;
        cardObject.transform.rotation = HandSpawnTransform.rotation;
        TheHandLayout.AddHand(cardObject.transform);

        // Init Card & Add to List
        CardController card = cardObject.GetComponent<CardController>();
        card.BindHandManager(this); // init
        HandCardList.Add(card); // add to list
    }

    private void destroyCard(CardController card)
    {
        // Remove from Layout
        TheHandLayout.RemoveHand(card.transform);


        // Remove from List & Destroy Card
        HandCardList.Remove(card);
        GameObject.Destroy(card.gameObject);
    }

    private void placeCard(CardController card, GridController grid)
    {
        // Remove from Layout
        TheHandLayout.RemoveHand(card.transform);

        // Move from HandList to GridList
        HandCardList.Remove(card);
        GridCardList.Add(card);
    }

    [Header("Hand Cards Info")]
    [ReadOnly]
    public List<CardController> HandCardList = new List<CardController>();
    [ReadOnly]
    public CardController hoveredCard = null;
    [ReadOnly]
    public CardController selectedCard = null;
    // View
    public void HoverCard(CardController card)
    {
        this.hoveredCard = card;
        TheHandLayout.HoverCard(card.transform);
    }

    public void SelectCard(CardController card)
    {
        this.selectedCard = card;
        TheHandLayout.SelectCard(card.transform);
    }

    public void ReleaseCard(CardController card)
    {
        if (this.selectedCard == card) this.selectedCard = null;
        TheHandLayout.ReleaseCard(card.transform);
    }

    public void ExitCard(CardController card)
    {
        if (this.hoveredCard == card) this.hoveredCard = null;
        TheHandLayout.UnhoverCard(card.transform);
    }


    [Header("Grid Cards Info")]
    public List<GridController> GridList = new List<GridController>();
    [ReadOnly]
    public List<CardController> GridCardList = new List<CardController>();


}
