using UnityEngine;
using Thnguyet.SaveGame;

[RequireComponent(typeof(Lever))]
public class LeverSaveData : SaveableComponent
{
    [Tooltip("Tình trạng đã dùng cần lưu.")]
    public bool used;

    [System.Serializable]
    private class LeverState
    {
        public bool used;
    }

    protected override void Reset()
    {
        base.Reset();
        if (string.IsNullOrWhiteSpace(saveKey))
            saveKey = gameObject.name + "_Lever";
    }

    public override object GetData()
    {
        return new LeverState { used = used };
    }

    public override void SetData(string data)
    {
        if (string.IsNullOrWhiteSpace(data))
            return;

        var loaded = JsonUtility.FromJson<LeverState>(data);
        if (loaded == null)
            return;

        used = loaded.used;
    }

    public override void OnAllDataLoaded()
    {
        if (!used)
            return;

        var lever = GetComponent<Lever>();
        if (lever != null)
            lever.ForceActivate();
    }
}
