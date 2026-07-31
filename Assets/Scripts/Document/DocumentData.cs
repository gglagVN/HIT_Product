using UnityEngine;

[CreateAssetMenu(fileName = "New Document", menuName = "Document/Document")]
public class DocumentData : ScriptableObject
{
    public string title;
    public Sprite paperImage;
}