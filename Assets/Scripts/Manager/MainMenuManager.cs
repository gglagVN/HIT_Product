using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void LoadNextScene()
    {
        SceneManager.LoadSceneAsync("Loading");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
