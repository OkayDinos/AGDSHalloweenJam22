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
        {
            Destroy(this);
            Destroy(gameObject);
        }
        else
            StaticManager.instance = this;

        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    public void Set(bool state)
    {
        if (staticImage != null)
        staticImage.enabled = state;
    }

    public async void StaticFor(float time)
    {
        float timer = 0f;

        Set(true);

        while (timer < time)
        {
            timer += Time.deltaTime;
            await System.Threading.Tasks.Task.Yield();
        }

        Set(false);
    }
}
