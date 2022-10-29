using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    public void GoBackButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
