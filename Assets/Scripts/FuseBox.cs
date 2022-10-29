using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FuseBox : Interactables
{
    public static FuseBox instance;

    private void Awake()
    {
        if (FuseBox.instance)
            Destroy(this);
        else
            FuseBox.instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void DoInteract()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<OkayDinos.GrimsNightmare.CharacterController>().hasFuse == true)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<OkayDinos.GrimsNightmare.CharacterController>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("WiresMinigame", LoadSceneMode.Additive);
        }
    }

    public void OnMinigameClosed()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<OkayDinos.GrimsNightmare.CharacterController>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync("WiresMinigame");
    }
}
