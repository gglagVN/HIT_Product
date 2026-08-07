using System.Collections.Generic;
using UnityEngine;
using Thnguyet.SaveGame;

public class DocumentSaveData : SaveableComponent
{
    [Tooltip("Danh sách tài liệu đã thu thập.")]
    public DocumentManager documentManager;

    [System.Serializable]
    private class DocumentDataSave
    {
        public List<string> documentNames = new List<string>();
    }

    protected override void Reset()
    {
        base.Reset();
        if (string.IsNullOrWhiteSpace(saveKey))
            saveKey = "DocumentState";
    }

    protected override void Awake()
    {
        ResolveDocumentManager();
        base.Awake();
    }

    private void ResolveDocumentManager()
    {
        if (documentManager != null)
            return;

        documentManager = FindObjectOfType<DocumentManager>();
        if (documentManager == null)
            documentManager = DocumentManager.Instance;
    }

    public override object GetData()
    {
        ResolveDocumentManager();
        var result = new DocumentDataSave();
        if (documentManager == null || documentManager.documents == null)
            return result;

        foreach (var doc in documentManager.documents)
        {
            if (doc != null)
                result.documentNames.Add(doc.name);
        }

        return result;
    }

    public override void SetData(string data)
    {
        ResolveDocumentManager();
        if (string.IsNullOrWhiteSpace(data) || documentManager == null)
            return;

        var loaded = JsonUtility.FromJson<DocumentDataSave>(data);
        if (loaded == null || loaded.documentNames == null)
            return;

        documentManager.documents.Clear();
        foreach (var name in loaded.documentNames)
        {
            var found = Resources.Load<DocumentData>(name);
            if (found != null)
            {
                documentManager.documents.Add(found);
                continue;
            }

            var allDocs = Resources.LoadAll<DocumentData>("");
            foreach (var doc in allDocs)
            {
                if (doc != null && doc.name == name)
                {
                    documentManager.documents.Add(doc);
                    break;
                }
            }
        }
    }
}
