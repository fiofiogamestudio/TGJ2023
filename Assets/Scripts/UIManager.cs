using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button NextTurnButton;
    public Text NextTurnTitle;
    public Text LevelText;
    void Awake()
    {
        NextTurnButton.onClick.AddListener(() =>
        {
            if (GameManager.instance.WaitEvents.Count == 0)
            {
                GameManager.instance.NextTurn();
            }
            else
            {
                GameManager.instance.ExecuteEvent();
            }
        });
    }



    void FixedUpdate()
    {
        if (GameManager.instance.WaitEvents.Count == 0)
        {
            NextTurnTitle.text = "下 个\n回 合";
        }
        else
        {
            EventType nextEventType = GameManager.instance.WaitEvents[0];
            string title = "";
            switch (nextEventType)
            {
                case EventType.GoodEvent:
                    title = "正 面\n事 件";
                    break;
                case EventType.BadEvent:
                    title = "负 面\n事 件";
                    break;
                case EventType.NormalEvent:
                    title = "中 立\n事 件";
                    break;
                case EventType.RandomEvent:
                    title = "随 机\n事 件";
                    break;
            }
            NextTurnTitle.text = title;
        }

        switch (GameConfig.gameType)
        {
            case GameType.Simple:
                LevelText.text = "安宁之家";
                break;
            case GameType.Normal:
                LevelText.text = "未知之地";
                break;
            case GameType.Hard:
                LevelText.text = "绝望之境";
                break;
        }
    }
}
