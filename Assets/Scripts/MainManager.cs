using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    MainMenu,
    InGame,
    InOptions,
    GameOver
}

class OptionsData
{
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
}

public class MainManager : MonoBehaviour
{
    public static MainManager instance;
    public GameState currentGameState;
    public GameState previousGameState;
    OptionsMenu optionsMenuRef;

    void Awake()
    {
        if (MainManager.instance)
            Destroy(this);
        else
            MainManager.instance = this;

        DontDestroyOnLoad(this);

        currentGameState = GameState.MainMenu;
        previousGameState = GameState.MainMenu;
    }

    void Start ()
    {
        if (File.Exists(Application.persistentDataPath + "/OptionsData.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/OptionsData.json");
            OptionsData data = JsonUtility.FromJson<OptionsData>(json);
            Volume.master = data.masterVolume;
            Volume.music = data.musicVolume;
            Volume.sfx = data.sfxVolume;

            Debug.Log($"Loaded options data {data.masterVolume}");
        }
        else
        {
            OptionsData data = new OptionsData();
            data.masterVolume = 0.5f;
            data.musicVolume = 0.5f;
            data.sfxVolume = 0.5f;
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(Application.persistentDataPath + "/OptionsData.json", json);

            Debug.Log($"Created new options data");
        }

        optionsMenuRef = OptionsMenu.instance;

        OptionsMenu.instance.UpdateSliders();
    }

    public void OpenOptions()
    {
        previousGameState = currentGameState;
        currentGameState = GameState.InOptions;
        optionsMenuRef.gameObject.SetActive(true);
        OptionsMenu.instance.UpdateSliders();
    }

    public void CloseOptions()
    {
        SaveData();
        currentGameState = previousGameState;
        optionsMenuRef.gameObject.SetActive(false);
    }

    public class Volume
    {
        public static float master = 0.5f;
        public static float music = 0.5f;
        public static float sfx = 0.5f;
    }

    void OnDestroy()
    {
        SaveData();
    }

    void SaveData()
    {
        OptionsData data = new OptionsData();
        data.masterVolume = Volume.master;
        data.musicVolume = Volume.music;
        data.sfxVolume = Volume.sfx;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/OptionsData.json", json);
    }
}
