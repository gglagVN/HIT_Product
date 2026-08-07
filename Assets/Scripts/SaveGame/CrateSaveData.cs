using UnityEngine;
using Thnguyet.SaveGame;

[RequireComponent(typeof(Crate))]
public class CrateSaveData : SaveableComponent
{
    [Tooltip("Đã giải mã crate chưa.")]
    public bool solved;

    [System.Serializable]
    private class CrateState
    {
        public bool solved;
    }

    protected override void Reset()
    {
        base.Reset();
        if (string.IsNullOrWhiteSpace(saveKey))
            saveKey = gameObject.name + "_Crate";
    }

    public override object GetData()
    {
        return new CrateState { solved = solved };
    }

    public override void SetData(string data)
    {
        if (string.IsNullOrWhiteSpace(data))
            return;

        var loaded = JsonUtility.FromJson<CrateState>(data);
        if (loaded == null)
            return;

        solved = loaded.solved;

        var crate = GetComponent<Crate>();
        if (crate == null)
            return;

        if (solved)
        {
            crate.MarkSolved();
        }
    }

    public override void OnAllDataLoaded()
    {
        if (!solved)
            return;

        var crate = GetComponent<Crate>();
        if (crate != null)
            crate.MarkSolved();
    }
}
