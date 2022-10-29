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

    public void ButtonStartGame()
    {
        MainManager.instance.currentGameState = GameState.InGame;
        SceneManager.LoadScene("Scenes/WiresMinigame");
    }
}
