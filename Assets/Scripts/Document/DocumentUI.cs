using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DocumentUI : MonoBehaviour
{
    public static DocumentUI Instance;

    [Header("UI")]

    public GameObject journal;
    public TextMeshProUGUI title;

    public Image paperImage;

    public Transform contentParent;

    public GameObject buttonPrefab;

    bool open = false;

    void Awake()
    {
        Instance = this;

        journal.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Toggle();
        }
    }

    public MonoBehaviour playerMovement;
    public PlayerLook playerLook;
    public DocumentViewer viewer;

    public void Toggle()
    {
        open = !open;

        journal.SetActive(open);
        viewer.ResetViewer();
        if (open)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            playerLook.SetLookEnabled(false);
            playerMovement.enabled = false;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            playerLook.SetLookEnabled(true);
            playerMovement.enabled = true;
        }
    }

    public void UpdateDocumentList()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (DocumentData doc in DocumentManager.Instance.documents)
        {
            GameObject obj = Instantiate(buttonPrefab, contentParent);

            obj.GetComponent<DocumentButton>().Setup(doc);
        }
    }

    public void ShowDocument(DocumentData doc)
    {
        paperImage.sprite = doc.paperImage;
        title.text = doc.title;
        viewer.ResetViewer();
    }
}