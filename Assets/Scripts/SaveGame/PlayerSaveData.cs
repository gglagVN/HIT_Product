using UnityEngine;
using Thnguyet.SaveGame;

public class PlayerSaveData : SaveableComponent
{
    [Tooltip("Player transform to save and restore.")]
    public Transform playerTransform;

    [Tooltip("Also restore player rotation when loading.")]
    public bool restoreRotation = true;

    [System.Serializable]
    private class PlayerData
    {
        public Vector3 position;
        public Quaternion rotation;
    }

    protected override void Reset()
    {
        base.Reset();
        if (string.IsNullOrWhiteSpace(saveKey))
            saveKey = "PlayerState";
    }

    protected override void Awake()
    {
        ResolvePlayerTransform();
        base.Awake();
    }

    private void ResolvePlayerTransform()
    {
        if (playerTransform != null)
            return;

        var playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            playerTransform = playerObject.transform;
    }

    public override object GetData()
    {
        ResolvePlayerTransform();
        if (playerTransform == null)
            return new PlayerData();

        return new PlayerData
        {
            position = playerTransform.position,
            rotation = playerTransform.rotation
        };
    }

    public override void SetData(string data)
    {
        ResolvePlayerTransform();
        if (string.IsNullOrWhiteSpace(data) || playerTransform == null)
            return;

        var saveData = JsonUtility.FromJson<PlayerData>(data);
        if (saveData == null)
            return;

        pendingPosition = saveData.position;
        pendingRotation = saveData.rotation;
        applyPending = true;
    }

    public override void OnAllDataLoaded()
    {
        ResolvePlayerTransform();
        if (!applyPending || playerTransform == null)
            return;

        playerTransform.position = pendingPosition;
        if (restoreRotation)
            playerTransform.rotation = pendingRotation;

        applyPending = false;
    }

    private Vector3 pendingPosition;
    private Quaternion pendingRotation;
    private bool applyPending;
}
