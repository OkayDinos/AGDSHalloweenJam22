using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    float timer = 10;

    Scene house, theForest;

    OkayDinos.GrimsNightmare.CharacterController playerHouse, playerForest;

    void Awake()
    {
        if (GameManager.instance)
            Destroy(this);
        else
            GameManager.instance = this;
    }

    void Start()
    {
        AsyncOperation house = SceneManager.LoadSceneAsync((int)SceneName.HOUSE, LoadSceneMode.Additive);
        AsyncOperation forest = SceneManager.LoadSceneAsync((int)SceneName.THEFOREST, LoadSceneMode.Additive);

        SetWhenDoneLoading();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            playerHouse.GoToTheForest();
        }
    }

    async void SetWhenDoneLoading()
    {
        while (!SceneManager.GetSceneByName("House").isLoaded)
        {
            await System.Threading.Tasks.Task.Yield();
        }

        while (!SceneManager.GetSceneByName("TheForest").isLoaded)
        {
            await System.Threading.Tasks.Task.Yield();
        }

        house = SceneManager.GetSceneByName("House");

        theForest = SceneManager.GetSceneByName("TheForest");

        FindPlayers();

        SetCurrentScene(SceneName.HOUSE);
    }

    public void SetCurrentScene(SceneName scene)
    {
        if (scene == SceneName.HOUSE)
        {
            SetInScene(house, true);
            SetInScene(theForest, false);
        }
        else if (scene == SceneName.THEFOREST)
        {
            SetInScene(house, false);
            SetInScene(theForest, true);

            playerForest.BeginForest();
        }
    }

    void SetInScene(Scene scene, bool active)
    {
        foreach (GameObject gameObject in scene.GetRootGameObjects())
        {
            gameObject.SetActive(active);
        }
    }

    void FindPlayers()
    {
        foreach (GameObject gameObject in house.GetRootGameObjects())
        {
            if (gameObject.GetComponent<OkayDinos.GrimsNightmare.CharacterController>() != null)
            {
                playerHouse = gameObject.GetComponent<OkayDinos.GrimsNightmare.CharacterController>();
            }
        }

        foreach (GameObject gameObject in theForest.GetRootGameObjects())
        {
            if (gameObject.GetComponent<OkayDinos.GrimsNightmare.CharacterController>() != null)
            {
                playerForest = gameObject.GetComponent<OkayDinos.GrimsNightmare.CharacterController>();
            }
        }
    }
}
