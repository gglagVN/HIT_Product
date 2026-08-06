using System.Collections;
using UnityEngine;

public class DocumentInterac : Interactable
{
    public DocumentData document;

    /// <summary>
    /// Chờ hết frame khởi tạo rồi bỏ vật phẩm nếu tài liệu đã được thu thập từ save.
    /// </summary>
    private IEnumerator Start()
    {
        yield return null;

        DocumentManager manager = DocumentManager.Instance;
        if (manager != null && document != null && manager.documents.Contains(document))
        {
            Destroy(gameObject);
        }
    }

    protected override void Interact()
    {
        DocumentManager.Instance.AddDocument(document);

        Destroy(gameObject);
    }
}