using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    public void GoBackButton()
    {
        StaticManager.instance.StaticFor(0.3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneName.MAINMENU);
    }
}
