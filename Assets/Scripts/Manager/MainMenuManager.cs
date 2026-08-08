using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Thnguyet.SaveGame;
using UnityEngine.UI;
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

        Button continueBT = startButtonLabel.GetComponentInParent<Button>();

        bool hasSave = SaveSystem.HasSave();

        continueBT.interactable = hasSave;
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
    public void DeleteSave()
    {
        try
        {
            SaveSystem.Delete();

            if (SaveGameManager.instance != null)
            {
                SaveGameManager.instance.DeleteAll();
            }

            PlayerPrefs.DeleteKey("EndingType");
            PlayerPrefs.Save();

            Debug.Log("SAVE DATA DELETED!");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"DeleteSave failed: {ex.Message}");
        }
    }
}
