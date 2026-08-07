using UnityEngine;
using Thnguyet.SaveGame;

public class AmmoBoxSaveData : SaveableComponent
{
    [Tooltip("Đã thu nhặt hộp đạn chưa.")]
    public bool collected;

    [System.Serializable]
    private class AmmoBoxState
    {
        public bool collected;
    }

    protected override void Reset()
    {
        base.Reset();
        if (string.IsNullOrWhiteSpace(saveKey))
            saveKey = gameObject.name + "_AmmoBox";
        mandatory = false;
    }

    public override object GetData()
    {
        return new AmmoBoxState { collected = collected };
    }

    public override void SetData(string data)
    {
        if (string.IsNullOrWhiteSpace(data))
            return;

        var loaded = JsonUtility.FromJson<AmmoBoxState>(data);
        if (loaded == null)
            return;

        collected = loaded.collected;
        gameObject.SetActive(!collected);
    }
}
