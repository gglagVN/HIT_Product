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
        badEnd.SetActive(false);
        goodEnd.SetActive(false);
        trueEnd.SetActive(false);

        int ending = PlayerPrefs.GetInt("EndingType", 0);

        switch (ending)
        {
            case 1:
                badEnd.SetActive(true);
                break;

            case 2:
                goodEnd.SetActive(true);
                break;

            case 3:
                trueEnd.SetActive(true);
                break;
        }
    }


    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ToggleTime()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        else if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
    }
}