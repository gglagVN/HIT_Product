using UnityEngine;
using Thnguyet.SaveGame;

[RequireComponent(typeof(Animator))]
public class OpenableSaveData : SaveableComponent
{
    [Tooltip("Animator parameter name that indicates this object is opened.")]
    public string openParameter = "isOpened";

    [System.Serializable]
    private class OpenStateData
    {
        public bool opened;
    }

    protected override void Reset()
    {
        base.Reset();
        if (string.IsNullOrWhiteSpace(saveKey))
            saveKey = gameObject.name + "_OpenState";
    }

    public override object GetData()
    {
        var anim = GetComponent<Animator>();
        return new OpenStateData
        {
            opened = anim != null && anim.GetBool(openParameter)
        };
    }

    public override void SetData(string data)
    {
        if (string.IsNullOrWhiteSpace(data))
            return;

        var loaded = JsonUtility.FromJson<OpenStateData>(data);
        if (loaded == null)
            return;

        var anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetBool(openParameter, loaded.opened);
    }
}
