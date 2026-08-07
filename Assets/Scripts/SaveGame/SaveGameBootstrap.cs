using System.Collections;
using UnityEngine;
using Thnguyet.SaveGame;

[DefaultExecutionOrder(200)]
public class SaveGameBootstrap : MonoBehaviour
{
    [Tooltip("Load save data automatically when the scene starts.")]
    public bool loadOnStart = true;

    [Tooltip("Save current state automatically when the scene starts.")]
    public bool saveOnStart = false;

    [Tooltip("Keep a backup copy of the previous save file.")]
    public bool useBackup = true;

    private void Awake()
    {
        if (FindObjectOfType<SaveLoadUI>() == null)
        {
            var go = new GameObject("[SaveLoadUI]");
            go.AddComponent<SaveLoadUI>();
            Debug.Log("[SaveGameBootstrap] Added SaveLoadUI fallback component.");
        }
    }

    private IEnumerator Start()
    {
        yield return null;

        if (loadOnStart)
        {
            SaveGameManager.instance.Load(true, true, useBackup);
            Debug.Log("[SaveGameBootstrap] Auto-load executed.");
        }

        if (saveOnStart)
        {
            SaveGameManager.instance.Save(true, true, useBackup);
            Debug.Log("[SaveGameBootstrap] Auto-save executed.");
        }
    }
}
