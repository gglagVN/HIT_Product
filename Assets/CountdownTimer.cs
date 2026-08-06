using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private float duration = 120f;

    private float currentTime;

    private bool isRunning;

    private int lastDisplayedSeconds = -1;

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
        lastDisplayedSeconds = -1;
        timerText.gameObject.SetActive(true);
    }

    private void UpdateTimerUI()
    {
        int totalSeconds = Mathf.FloorToInt(currentTime);

        if (totalSeconds == lastDisplayedSeconds)
            return;

        lastDisplayedSeconds = totalSeconds;

        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.SetText("{0:00}:{1:00}", minutes, seconds);
    }
}