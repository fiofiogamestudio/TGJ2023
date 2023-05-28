using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button NextTurnButton;

    void Awake()
    {
        NextTurnButton.onClick.AddListener(() =>
        {
            GameManager.instance.NextTurn();
        });
    }
}
