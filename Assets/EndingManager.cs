using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndingManager : MonoBehaviour
{
    [Header("Ending UI")]
    [SerializeField] private GameObject badEnd;
    [SerializeField] private GameObject goodEnd;
    [SerializeField] private GameObject trueEnd;

    private void Start()
    {
        ShowEnding();
    }

    private void ShowEnding()
    {
        SetPanelActive(badEnd, false);
        SetPanelActive(goodEnd, false);
        SetPanelActive(trueEnd, false);

        int ending = PlayerPrefs.GetInt("EndingType", 0);

        switch (ending)
        {
            case 1:
                SetPanelActive(badEnd, true);
                break;

            case 2:
                SetPanelActive(goodEnd, true);
                break;

            case 3:
                SetPanelActive(trueEnd, true);
                break;
        }
    }

    private void SetPanelActive(GameObject panel, bool active)
    {
        if (panel == null)
        {
            if (active)
            {
                Debug.LogError("EndingManager: chưa gán panel kết thúc trong Inspector.", this);
            }
            return;
        }

        panel.SetActive(active);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}