using UnityEngine;
using Thnguyet.SaveGame;

public abstract class SaveableComponent : MonoBehaviour, ISaveData
{
    [Tooltip("Unique save key used to identify this object. If empty, GameObject.name will be used.")]
    public string saveKey;

    [Tooltip("Save as mandatory data.")]
    public bool mandatory = true;

    public bool DataChanged { get; set; } = true;

    protected virtual void Reset()
    {
        if (string.IsNullOrWhiteSpace(saveKey))
            saveKey = gameObject.name;
    }

    protected virtual void Awake()
    {
        if (string.IsNullOrWhiteSpace(saveKey))
            saveKey = gameObject.name;

        RegisterSaveData();
    }

    public virtual void RegisterSaveData()
    {
        if (mandatory)
            SaveGameManager.instance.RegisterMandatoryData(saveKey, this);
        else
            SaveGameManager.instance.RegisterOptionalData(saveKey, this);
    }

    public abstract object GetData();

    public abstract void SetData(string data);

    public virtual void OnAllDataLoaded() { }
}
