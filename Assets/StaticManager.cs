using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticManager : MonoBehaviour
{
    public static StaticManager instance;

    public Image staticImage;

    void Awake()
    {
        if (StaticManager.instance)
            Destroy(this);
        else
            StaticManager.instance = this;

        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    public void Set(bool state)
    {
        staticImage.enabled = state;
    }
}
