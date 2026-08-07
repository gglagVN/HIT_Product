using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Thnguyet.SaveGame;
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
    public void DeleteSave() { if (SaveGameManager.instance == null) { Debug.LogWarning("SaveGameManager instance not available."); return; } SaveGameManager.instance.DeleteAll(); Debug.Log("SAVE DATA DELETED!"); }
}
