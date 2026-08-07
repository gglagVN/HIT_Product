using UnityEngine;

[CreateAssetMenu(fileName = "DocumentDatabase", menuName = "SaveGame/DocumentDatabase")]
public class DocumentDatabase : ScriptableObject
{
    public DocumentData[] allDocuments;

    public DocumentData FindDocumentByName(string documentName)
    {
        if (allDocuments == null)
            return null;

        for (int i = 0; i < allDocuments.Length; i++)
        {
            if (allDocuments[i] != null && allDocuments[i].name == documentName)
                return allDocuments[i];
        }

        return null;
    }
}
