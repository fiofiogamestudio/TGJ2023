using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUIManager : MonoBehaviour
{
    public Text TimeText;
    public Text PeopleText;
    public Text FoodText;

    void FixedUpdate()
    {
        // Listen
        refreshStatusView();
    }

    void refreshStatusView()
    {
        refreshTimeText();
        refreshPeopleText();
        refreshFoodText();
    }

    void refreshTimeText()
    {
        int turn = GameManager.instance.TurnCount;
        turn = (turn - 1) % 36 + 1;
        int season = (turn - 1) / 9 + 1;   // 季度是9个turn一轮回
        int month = ((turn - 1) / 3) % 4 + 1;  // 月份是3个turn一轮回, 4个月一季度
        int monthPart = (turn - 1) % 3 + 1; // 每月分为上中下旬
        TimeText.text = numberToSeason(season) + " " +
                        numberToMonth(month) + "月 " +
                        numberToMonthPart(monthPart);
    }


    string numberToSeason(int num)
    {
        switch (num)
        {
            case 1: return "春";
            case 2: return "夏";
            case 3: return "秋";
            case 4: return "冬";
            default: return "?";
        }
    }


    string numberToMonth(int num)
    {
        switch (num)
        {
            case 1: return "一";
            case 2: return "二";
            case 3: return "三";
            case 4: return "四";
            case 5: return "五";
            case 6: return "六";
            case 7: return "七";
            case 8: return "八";
            case 9: return "九";
            case 10: return "十";
            case 11: return "十一";
            case 12: return "十二";
            default: return "?";
        }
    }

    string numberToMonthPart(int num)
    {
        switch (num)
        {
            case 1: return "上旬";
            case 2: return "中旬";
            case 3: return "下旬";
            default: return "?";
        }
    }


    void refreshPeopleText()
    {
        PeopleText.text = GameManager.instance.PeopleCount.ToString();
    }

    void refreshFoodText()
    {
        FoodText.text = GameManager.instance.FoodCount.ToString();
    }

}
