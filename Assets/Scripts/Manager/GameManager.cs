using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Pause Menu")]
    public GameObject pausePanel;

    private bool isPaused = false;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Đảm bảo game bắt đầu ở trạng thái bình thường
        Time.timeScale = 1f;
    }

    private void Start()
    {
        pausePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;

        // Hiện Pause Panel
        pausePanel.SetActive(true);

        // Dừng game
        Time.timeScale = 0f;

        // Hiện chuột
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;

        // Tắt Pause Panel
        pausePanel.SetActive(false);

        // Tiếp tục game
        Time.timeScale = 1f;

        // Khóa chuột lại
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}