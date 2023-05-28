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

        // 检测切换窗口导致的Release
        checkSwitchWindow();

        // 检测鼠标是否在Grid上
        checkHoveringGrid();

        // 检测鼠标是否选中Grid且上面有Card
        checkSelectGrid();

    }

    public GridController hoveredGrid = null;

    private void checkSwitchWindow()
    {
        if (!InputManager.GetMouseHolding())
        {
            if (selectedCard) ReleaseCard(selectedCard, switchWindow: true);
        }
    }

    private void checkHoveringGrid()
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

    private void checkSelectGrid()
    {
        if (InputManager.GetMouseDown())
        {
            if (selectedCard && selectedCard.binding)
            {
                pickCard(selectedCard, backToHand: false);
            }
        }
        else if (InputManager.GetMouseRightDown())
        {
            selectedCard = hoveredCard;
            if (selectedCard && selectedCard.binding)
            {
                pickCard(selectedCard, backToHand: true);
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
        TheHandLayout.ReleaseCard(card.transform);
        TheHandLayout.RemoveHand(card.transform);

        // Move from HandList to GridList
        HandCardList.Remove(card);
        GridCardList.Add(card);

        // card & grid bind
        card.BindGrid(grid);
        grid.BindCard(card);
    }

    private void pickCard(CardController card, bool backToHand = true)
    {
        // Add to Layout
        TheHandLayout.AddHand(card.transform);
        if (!backToHand)
        {
            TheHandLayout.SelectCard(card.transform);
        }
        else
        {
            ReleaseCard(card);
        }

        // Move from GridList to HandList
        HandCardList.Add(card);
        GridCardList.Remove(card);

        // card & grid unbind
        card.bindedGrid.UnBind();
        card.UnBind();
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
        if (!card.binding)
        {
            TheHandLayout.HoverCard(card.transform);
        }
    }

    public void SelectCard(CardController card)
    {
        this.selectedCard = card;
        if (!card.binding)
        {
            TheHandLayout.SelectCard(card.transform);
        }
    }

    public void ReleaseCard(CardController card, bool switchWindow = false)
    {
        // 先处理放置卡牌的逻辑
        if (!switchWindow)
        {
            handlePlaceCard();
        }

        if (this.selectedCard == card) this.selectedCard = null;
        if (!card.binding)
        {
            TheHandLayout.ReleaseCard(card.transform);
        }
    }

    void handlePlaceCard()
    {
        if (selectedCard && hoveredGrid)
        {
            if (!hoveredGrid.binding)
            {
                PlaceCard(selectedCard, hoveredGrid);
            }
        }
    }

    public void ExitCard(CardController card)
    {
        if (this.hoveredCard == card) this.hoveredCard = null;
        if (!card.binding)
        {
            TheHandLayout.UnhoverCard(card.transform);
        }
    }


    [Header("Grid Cards Info")]
    public List<GridController> GridList = new List<GridController>();
    [ReadOnly]
    public List<CardController> GridCardList = new List<CardController>();


}
