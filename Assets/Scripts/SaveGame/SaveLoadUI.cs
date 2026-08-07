using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadUI : MonoBehaviour
{
    [Header("Save/Load Options")]
    public bool includeMandatory = true;
    public bool includeOptional = true;
    public bool useBackup = true;
    public bool autoSaveOnQuit = false;
    public bool enableHotkeys = true;

    private void Awake()
    {
        Debug.Log("[SaveLoadUI] Ready. Press F5 to Save, F6 to Load.");
    }

    private void Update()
    {
        if (!enableHotkeys)
            return;

        if (Input.GetKeyDown(KeyCode.F5))
        {
            OnSaveButton();
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            OnLoadButton();
        }
    }

    // Called from UI button OnClick
    public void OnSaveButton()
    {
        Debug.Log(Application.persistentDataPath);
        if (Thnguyet.SaveGame.SaveGameManager.instance != null)
        {
            Thnguyet.SaveGame.SaveGameManager.instance.Save(includeMandatory, includeOptional, useBackup);
            // store last active scene so we can load back to it later
            try
            {
                var folder = Path.Combine(Application.persistentDataPath, "SaveGame");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                var path = Path.Combine(folder, "lastscene.txt");
                File.WriteAllText(path, SceneManager.GetActiveScene().name);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("SaveLoadUI: failed to write lastscene: " + e.Message);
            }
            Debug.Log("SaveGame: Save requested.");
        }
        else
        {
            Debug.LogWarning("SaveGameManager instance not available.");
        }
    }

    // Called from UI button OnClick
    public void OnLoadButton()
    {
        if (Thnguyet.SaveGame.SaveGameManager.instance != null)
        {
            Debug.Log("SaveGame: Load requested.");
            StartCoroutine(ApplyLoadAfterFrame());
        }
        else
        {
            Debug.LogWarning("SaveGameManager instance not available.");
        }
    }

    private System.Collections.IEnumerator ApplyLoadAfterFrame()
    {
        yield return null;

        string savedScene = null;
        try
        {
            var path = Path.Combine(Application.persistentDataPath, "SaveGame", "lastscene.txt");
            if (File.Exists(path)) savedScene = File.ReadAllText(path);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("SaveLoadUI: failed to read lastscene: " + e.Message);
        }

        if (!string.IsNullOrEmpty(savedScene) && savedScene != SceneManager.GetActiveScene().name)
        {
            SceneManager.sceneLoaded += OnSceneLoadedApplySave;
            SceneManager.LoadScene(savedScene);
            yield break;
        }

        Thnguyet.SaveGame.SaveGameManager.instance.Load(includeMandatory, includeOptional, true);
        yield return null;
        Thnguyet.SaveGame.SaveGameManager.instance.Load(includeMandatory, includeOptional, true);
    }

    private static void OnSceneLoadedApplySave(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoadedApplySave;
        if (Thnguyet.SaveGame.SaveGameManager.instance != null)
        {
            Thnguyet.SaveGame.SaveGameManager.instance.Load(true, true, true);
            Debug.Log("SaveGame: Loaded scene and applied saved data.");
        }
        else
        {
            Debug.LogWarning("SaveGameManager instance not available after scene load.");
        }
    }

    private void OnApplicationQuit()
    {
        if (autoSaveOnQuit)
        {
            if (Thnguyet.SaveGame.SaveGameManager.instance != null)
            {
                Thnguyet.SaveGame.SaveGameManager.instance.Save(includeMandatory, includeOptional, useBackup);
                Debug.Log("SaveGame: Auto-saved on quit.");
            }
        }
    }
}
