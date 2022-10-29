using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    MainMenu,
    InGame,
    InOptions,
    GameOver
}
public class MainManager : MonoBehaviour
{
    public static MainManager instance;
    public GameState currentGameState = GameState.MainMenu;
    OptionsMenu optionsMenuRef;

    void Awake()
    {
        if (MainManager.instance)
            Destroy(this);
        else
            MainManager.instance = this;

        DontDestroyOnLoad(this);
    }

    void Start ()
    {
        optionsMenuRef = OptionsMenu.instance;
    }

    public void OpenOptions()
    {
        currentGameState = GameState.InOptions;
        optionsMenuRef.gameObject.SetActive(true);
    }

    public void CloseOptions()
    {
        currentGameState = GameState.MainMenu;
        optionsMenuRef.gameObject.SetActive(false);
    }
}
