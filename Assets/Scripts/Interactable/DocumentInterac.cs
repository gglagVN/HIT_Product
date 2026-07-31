using UnityEngine;

public class DocumentInterac : Interactable
{
    public DocumentData document;

    protected override void Interact()
    {
        DocumentManager.Instance.AddDocument(document);

        Destroy(gameObject);
    }
}