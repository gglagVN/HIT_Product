using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Thnguyet.SaveGame;
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Text startButtonLabel;

    private void Start()
    {
        if (startButtonLabel == null)
        {
            Debug.LogError("MainMenuManager: chưa gán startButtonLabel trong Inspector.", this);
            return;
        }

        startButtonLabel.text = SaveSystem.HasSave() ? "CONTINUE" : "NEW GAME";
    }

    public void LoadNextScene()
    {
        SceneManager.LoadSceneAsync("Loading");
    }

    /// Chuyển sang scene bất kỳ theo tên, dùng cho nút quay về menu ở màn kết thúc.
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void DeleteSave() { if (SaveGameManager.instance == null) { Debug.LogWarning("SaveGameManager instance not available."); return; } SaveGameManager.instance.DeleteAll(); Debug.Log("SAVE DATA DELETED!"); }
}
