using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (endGame && Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuScene");
        }
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
        int emergencyCount = HandManager.instance.countEmergencyCardsInHand() + HandManager.instance.countEmergencyCardsInGrid();
        if (emergencyCount > 0 && emergencyCount <= PeopleCount)
        {
            Debug.LogWarning("Emergency in hand");
            return;
        }

        if (emergencyCount > PeopleCount)
        {
            Debug.LogWarning("危机过多！");
            endGame = true;
            EmergencyEnd.gameObject.SetActive(true);
            return;
        }

        Debug.Log("Next Turn");
        HandManager.instance.ExecuteCards();



        // Eat Food
        FoodCount -= PeopleCount;

        if (FoodCount < 0)
        {
            Debug.LogWarning("粮食耗尽！");
            endGame = true;
            FoodEnd.gameObject.SetActive(true);
            return;
        }

        // turn count + 1
        TurnCount++;
        if ((TurnCount - 1) % 3 == 0) AddEvent(EventType.RandomEvent);
    }


    [Header("Game Info")]
    public int PeopleCount = 5;
    public int FoodCount = 30;
    public int TurnCount = 1;


    public void AddFood(int count)
    {
        FoodCount += count;
    }

    public void AddPeople(int count)
    {
        PeopleCount++;
        if (PeopleCount > 15)
        {
            endGame = true;
            PeopleEnd.gameObject.SetActive(true);
        }
        HandManager.instance.AddGrid();
    }


    // public void AddEvent()
    [Header("Events Info")]
    [ReadOnly]
    public List<EventType> WaitEvents = new List<EventType>();
    public EventPopController EventPop;

    public void AddEvent(EventType type)
    {
        WaitEvents.Add(type);
    }

    public void ExecuteEvent()
    {
        EventType type = WaitEvents[0];

        Debug.Log("Execute Event " + type.ToString());
        popEvent(type);

        WaitEvents.RemoveAt(0);
    }

    private void popEvent(EventType type)
    {
        EventData eventData;
        switch (type)
        {
            case EventType.GoodEvent:
                eventData = EventHolder.instance.GetGoodEvent();
                break;
            case EventType.BadEvent:
                eventData = EventHolder.instance.GetBadEvent();
                break;
            case EventType.NormalEvent:
                eventData = EventHolder.instance.GetNormalEvent();
                break;
            case EventType.RandomEvent:
                eventData = EventHolder.instance.GetRandomEvent(CurrentGameData);
                break;
            default:
                eventData = EventHolder.instance.GetRandomEvent(CurrentGameData);
                break;
        }
        EventPop.InitData(eventData);
        EventPop.gameObject.SetActive(true);
    }


    [Header("End Info")]
    public GameObject FoodEnd;
    public GameObject EmergencyEnd;
    public GameObject PeopleEnd;


    public bool endGame = false;
}
