using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private float duration = 120f;

    private float currentTime;

    private bool isRunning;

    private int lastDisplayedSeconds = -1;

    public float Remaining => currentTime;

    public bool IsRunning => isRunning;

    private void Start()
    {
        if (isRunning)
            return;

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

    /// <summary>
    /// Chạy tiếp đếm ngược từ số giây còn lại đã lưu.
    /// </summary>
    public void ResumeCountdown(float remaining)
    {
        currentTime = Mathf.Clamp(remaining, 0f, duration);
        isRunning = currentTime > 0f;
        lastDisplayedSeconds = -1;
        timerText.gameObject.SetActive(true);
        UpdateTimerUI();
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