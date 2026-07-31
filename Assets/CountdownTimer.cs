using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private float duration = 120f;

    private float currentTime;

    private bool isRunning;

    private void Start()
    {
        currentTime = duration;
        UpdateTimerUI();
        timerText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isRunning)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            isRunning = false;

            // TODO: Nổ map
            Debug.Log("BOOM!");
        }

        UpdateTimerUI();
    }

    public void StartCountdown()
    {
        currentTime = duration;
        isRunning = true;
        timerText.gameObject.SetActive(true);
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}