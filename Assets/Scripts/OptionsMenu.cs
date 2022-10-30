using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public static OptionsMenu instance;

    [SerializeField] GameObject menuButton;

    [SerializeField] List<GameObject> volumes = new List<GameObject>();

    void Awake()
    {
        if (OptionsMenu.instance)
        {
            Destroy(this);
            Destroy(gameObject);
        }
        else
            OptionsMenu.instance = this;

        DontDestroyOnLoad(this);
    }

    void OnEnable()
    {
        if (MainManager.instance != null)
        {
            if (MainManager.instance.previousGameState == GameState.InGame)
            {
                menuButton.SetActive(true);
                if (Wires.instance != null)
                Wires.instance.Pause();
            }
            else
            {
                menuButton.SetActive(false);
            }
        }

        UpdateSliders();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonMainMenu()
    {
        MainManager.instance.CloseOptions();
        MainManager.instance.currentGameState = GameState.MainMenu;
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneName.MAINMENU);

        Cursor.lockState = CursorLockMode.None;
    }

    public void ButtonBack()
    {
        if (MainManager.instance.previousGameState == GameState.InGame)
        {
            if (Wires.instance != null)
            Wires.instance.Unpause();
        }
        MainManager.instance.CloseOptions();
    }

    public void UpdateSliders()
    {
        volumes[0].GetComponent<Slider>().value = MainManager.Volume.master;
        volumes[1].GetComponent<Slider>().value = MainManager.Volume.music;
        volumes[2].GetComponent<Slider>().value = MainManager.Volume.sfx;
    }

    public void UpdateVolume(int _sliderIndex)
    {
        if (_sliderIndex == 0)
        {
            MainManager.Volume.master = volumes[0].GetComponent<Slider>().value;
        }
        else if (_sliderIndex == 1)
        {
            MainManager.Volume.music = volumes[1].GetComponent<Slider>().value;
        }
        else if (_sliderIndex == 2)
        {
            MainManager.Volume.sfx = volumes[2].GetComponent<Slider>().value;
        }
    }
}
