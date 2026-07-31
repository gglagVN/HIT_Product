using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DocumentButton : MonoBehaviour
{
    private DocumentData data;

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Image thumbnail;

    private Button button;

    public void Setup(DocumentData doc)
    {
        if (button == null)
            button = GetComponent<Button>();

        data = doc;

        titleText.text = doc.title;
        thumbnail.sprite = doc.paperImage;   // <-- Hiển thị ảnh

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OpenDocument);
    }

    private void OpenDocument()
    {
        DocumentUI.Instance.ShowDocument(data);
    }
}