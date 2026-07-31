using System.Collections.Generic;
using UnityEngine;

public class DocumentManager : MonoBehaviour
{
    public static DocumentManager Instance;

    public List<DocumentData> documents = new();

    private void Awake()
    {
        Instance = this;
    }

    public void AddDocument(DocumentData doc)
    {
        if (documents.Contains(doc))
            return;

        documents.Add(doc);

        DocumentUI.Instance.UpdateDocumentList();
    }
}