using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [ReadOnly]
    public GameData CurrentGameData;


    void Awake()
    {
        if (instance == null) instance = this;
        loadGameData();
    }

    void Start()
    {
        initGame();
    }

    void loadGameData()
    {
        GameWrapper gameWrapper = DataLoader.LoadJson<GameWrapper>("Game");
        switch (GameConfig.gameType)
        {
            case GameType.Simple:
                CurrentGameData = gameWrapper.simpleGameData;
                break;
            case GameType.Normal:
                CurrentGameData = gameWrapper.normalGameData;
                break;
            case GameType.Hard:
                CurrentGameData = gameWrapper.hardGameData;
                break;
        }


    }

    void initGame()
    {
        // init mission in hand
        foreach (var missionName in CurrentGameData.initMissions)
        {
            HandManager.instance.DrawCard(missionName);
        }
    }


    public void NextTurn()
    {
        Debug.Log("Next Turn");
        HandManager.instance.ExecuteCards();




        // Eat Food
        FoodCount -= PeopleCount;

        // turn count + 1
        TurnCount++;
    }


    [Header("Game Info")]
    public int PeopleCount = 5;
    public int FoodCount = 25;
    public int TurnCount = 1;


    public void AddFood(int count)
    {
        FoodCount += count;
    }

    public void AddPeople(int count)
    {
        PeopleCount++;
        HandManager.instance.AddGrid();
    }

}
