using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public HandLayout TheHandLayout;
    public Transform HandSpawnTransform;
    public Transform HandDestroyTransform;
    public GameObject CardPrefab;

    public static HandManager instance;
    void Awake()
    {
        if (instance == null) instance = this;
    }


    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     GenerateCard();
        // }

        // if (Input.GetKeyDown(KeyCode.D))
        // {
        //     DiscardCard(HandCardList[0]);
        // }

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

    public void GenerateCard(MissionData data)
    {
        spawnCard(data, draw: false);
    }

    public void GenerateCard(string name)
    {
        MissionData data = MissionHolder.instance.GetMissionData(name);
        GenerateCard(data);
    }

    public void DrawCard(MissionData data)
    {
        spawnCard(data, draw: true);
    }

    public void DrawCard(string name)
    {
        Debug.Log("Draw " + name);
        MissionData data = MissionHolder.instance.GetMissionData(name);
        DrawCard(data);
    }

    public void DiscardCard(CardController card)
    {
        destroyCard(card);
    }

    public void PlaceCard(CardController card, GridController grid)
    {
        placeCard(card, grid);
    }

    private void spawnCard(MissionData data, bool draw = false)
    {
        GameObject cardObject = GameObject.Instantiate(CardPrefab);
        // Add to Layout
        cardObject.transform.SetParent(TheHandLayout.transform);
        cardObject.transform.position = HandSpawnTransform.position;
        cardObject.transform.rotation = HandSpawnTransform.rotation;
        TheHandLayout.AddHand(cardObject.transform);

        // Init Card & Add to List
        CardController card = cardObject.GetComponent<CardController>();
        card.InitData(data);
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

        // cost
        CardEffectManager.instance.ExecuteEffects(card.missionCosts);
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

    public GameObject GridPrefab;

    public GameObject TheGridLayout;

    public void AddGrid()
    {
        GameObject gridObject = GameObject.Instantiate(GridPrefab);
        gridObject.transform.SetParent(TheGridLayout.transform);
        gridObject.transform.localScale = Vector3.one;
        gridObject.transform.localRotation = Quaternion.identity;
        // add to list
        GridList.Add(gridObject.GetComponent<GridController>());
    }

    public void RemoveGrid(GridController grid)
    {
        GridList.Remove(grid);

        GameObject.Destroy(grid.gameObject);
    }



    // Execute Cards

    public void ExecuteCards()
    {
        // check emergency cards
        if (checkEmergencyCardsInHand())
        {
            Debug.LogWarning("Emergency!");
            return;
        }

        // discard time card in hands
        discardTimeCardsInHand();

        // execute card in grid
        executeCardsInGrid();
    }

    private bool checkEmergencyCardsInHand()
    {
        foreach (var card in HandCardList)
        {
            if (card.missionType == MissionType.Emergency) return true;
        }
        return false;
    }

    private void discardTimeCardsInHand()
    {
        List<CardController> toDiscardList = new List<CardController>();
        foreach (var card in HandCardList)
        {
            if (card.missionType == MissionType.Normal && card.missionLeft >= 0)
            {
                toDiscardList.Add(card);
            }
        }
        foreach (var card in toDiscardList)
        {
            DiscardCard(card);
        }
    }

    private void executeCardsInGrid()
    {
        List<CardController> toDestroyCardList = new List<CardController>();

        foreach (var card in GridCardList)
        {
            card.missionProgress -= 1;
            CardEffectManager.instance.ExecuteEffects(card.turnEffects);
            if (card.missionProgress <= 0)
            {
                // execute end effects
                CardEffectManager.instance.ExecuteEffects(card.endEffects);
                if (card.missionType == MissionType.Continues)
                {
                    // card.InitData()
                    card.ResetData();
                }
                else
                {
                    toDestroyCardList.Add(card);
                }

            }

            card.RefreshView();
        }

        foreach (var card in toDestroyCardList)
        {
            selectedCard = card;
            if (selectedCard && selectedCard.binding)
            {
                pickCard(selectedCard, backToHand: true);
            }
            DiscardCard(card);
        }
    }

}
