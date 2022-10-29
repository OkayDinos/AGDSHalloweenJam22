using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public static OptionsMenu instance;

    void Awake()
    {
        if (OptionsMenu.instance)
            Destroy(this);
        else
            OptionsMenu.instance = this;

        DontDestroyOnLoad(this);

        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonBack()
    {
        MainManager.instance.CloseOptions();
    }
}
