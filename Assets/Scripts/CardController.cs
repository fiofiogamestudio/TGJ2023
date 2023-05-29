using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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




    [Header("Card Info")]
    [ReadOnly]
    public MissionType missionType;
    [ReadOnly]
    public string missionName;
    [ReadOnly]
    public string missoinDescription;
    [ReadOnly]
    public int missionAmount;
    [ReadOnly]
    public int missionProgress;
    [ReadOnly]
    public int missionLeft;
    [ReadOnly]
    public string[] missionCosts;
    [ReadOnly]
    public string[] turnEffects;
    [ReadOnly]
    public string[] endEffects;

    // Init Data
    public void InitData(MissionData data)
    {
        missionType = data.missionType;
        missionName = data.name;
        missoinDescription = data.description;
        missionAmount = data.amount;
        missionProgress = data.amount;
        missionLeft = data.left;
        missionCosts = data.costs;
        turnEffects = data.turnEffects;
        endEffects = data.endEffects;

        initView();
    }

    public void ResetData()
    {
        missionProgress = missionAmount;
    }

    [Header("Card View")]
    public Image CardTitleBG;
    public Text CardNameText;
    public Image CardImage;
    public Text CardDescText;
    public Image ContinuesIcon;
    public Image NormalIcon;
    public Image TimeIcon;
    public Image EmergencyIcon;

    public Text CardProgressText;


    private void initView()
    {
        switch (missionType)
        {
            case MissionType.Normal:
                if (missionLeft < 0)
                {
                    // 普通任务
                    CardTitleBG.color = ColorHolder.instance.NormalMissionColor;
                    NormalIcon.gameObject.SetActive(true);
                    NormalIcon.color = ColorHolder.instance.NormalMissionColor;
                }
                else
                {
                    // 限时任务
                    CardTitleBG.color = ColorHolder.instance.TimeMissonColor;
                    TimeIcon.gameObject.SetActive(true);
                    TimeIcon.color = ColorHolder.instance.TimeMissonColor;
                }
                break;
            case MissionType.Continues:
                CardTitleBG.color = ColorHolder.instance.ContinuesMissionColor;
                ContinuesIcon.gameObject.SetActive(true);
                ContinuesIcon.color = ColorHolder.instance.ContinuesMissionColor;
                break;
            case MissionType.Emergency:
                CardTitleBG.color = ColorHolder.instance.EmergencyMissionColor;
                EmergencyIcon.gameObject.SetActive(true);
                ContinuesIcon.color = ColorHolder.instance.EmergencyMissionColor;
                break;
        }

        CardNameText.text = missionName;
        CardImage.sprite = CardSpriteHolder.instance.GetCardSprite(missionName);
        CardDescText.text = missoinDescription;
        CardProgressText.text = missionAmount.ToString();
    }

    public void RefreshView()
    {
        CardProgressText.text = missionProgress.ToString();
    }
}
