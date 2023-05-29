using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    public Button SimpleButton;
    public Button NormalButton;
    public Button HardButton;

    void Awake()
    {
        Screen.SetResolution(1920, 1080, false);

        SimpleButton.onClick.AddListener(() =>
        {
            GameConfig.gameType = GameType.Simple;
            SceneManager.LoadScene("GameScene");
        });

        NormalButton.onClick.AddListener(() =>
        {
            GameConfig.gameType = GameType.Normal;
            SceneManager.LoadScene("GameScene");
        });

        HardButton.onClick.AddListener(() =>
        {
            GameConfig.gameType = GameType.Hard;
            SceneManager.LoadScene("GameScene");
        });

    }
}
