using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MainManager.instance.CloseOptions();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonOptions()
    {
        MainManager.instance.OpenOptions();
    }

    public void ButtonQuitGame()
    {
        Debug.Log("Game closed");
        Application.Quit();
    }

    public void ButtonSus()
    {
        MainManager.instance.currentGameState = GameState.InGame;
        SceneManager.LoadScene((int)SceneName.WIRESMINIGAME);
    }

    public void FunnySoundButton()
    {

    }

    public void ButtonStartGame()
    {
        MainManager.instance.currentGameState = GameState.InGame;
        SceneManager.LoadScene((int)SceneName.HOUSE);
        StaticManager.instance.StaticFor(0.3f);
    }

    public void ButtonOpenCredits()
    {
        SceneManager.LoadScene((int)SceneName.CREDITS);
        StaticManager.instance.StaticFor(0.3f);
    }
}
