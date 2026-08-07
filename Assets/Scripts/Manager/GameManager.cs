using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameSaveData
{
    public int saveVersion;
    public Vector3 playerPosition;
    public float playerYaw;
    public float cameraPitch;
    public float playerHealth;
    public int currentWeapon;
    public bool[] gunUnlocked;
    public int[] gunBulletsLeft;
    public int[] gunAmmoReserve;
    public string[] collectedDocuments;
    public string[] openedObjectIds;
    public bool leverUsed;
    public float countdownRemaining;
}

public static class SaveSystem
{
    private const string SaveFileName = "save.json";

    private static string FilePath => Path.Combine(Application.persistentDataPath, SaveFileName);

    /// <summary>
    /// Trả về true khi có file save đọc được và parse thành công.
    /// </summary>
    public static bool HasSave()
    {
        return Load() != null;
    }

    /// <summary>
    /// Đọc save từ đĩa, trả về null nếu không có file hoặc file hỏng.
    /// </summary>
    public static GameSaveData Load()
    {
        try
        {
            if (!File.Exists(FilePath))
            {
                return null;
            }

            string json = File.ReadAllText(FilePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            return JsonUtility.FromJson<GameSaveData>(json);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"SaveSystem: không đọc được save ({e.Message}), coi như chưa có save.");
            return null;
        }
    }

    /// <summary>
    /// Ghi save xuống đĩa dưới dạng JSON.
    /// </summary>
    public static void Save(GameSaveData data)
    {
        if (data == null)
        {
            return;
        }

        try
        {
            File.WriteAllText(FilePath, JsonUtility.ToJson(data, true));
        }
        catch (Exception e)
        {
            Debug.LogError($"SaveSystem: ghi save thất bại ({e.Message}).");
        }
    }

    /// <summary>
    /// Xoá file save hiện có.
    /// </summary>
    public static void Delete()
    {
        try
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"SaveSystem: xoá save thất bại ({e.Message}).");
        }
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private const int CurrentSaveVersion = 1;

    [Header("Pause Menu")]
    public GameObject pausePanel;
    [SerializeField] private SetOnOffPanel settingsPanel;

    [Header("Save References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerLook playerLook;
    [SerializeField] private GunHolder gunHolder;
    [SerializeField] private DocumentManager documentManager;
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private MonoBehaviour playerMovement;
    [SerializeField] private InputManager inputManager;

    private CharacterController playerController;
    private bool isPaused = false;
    private bool isReloadingScene = false;

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

        ValidateSaveReferences();

        if (playerTransform != null)
        {
            playerController = playerTransform.GetComponent<CharacterController>();
        }
    }

    private IEnumerator Start()
    {
        pausePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Chờ 1 frame cho Awake/Start của các script khác chạy xong rồi mới ghi đè state
        yield return null;

        GameSaveData data = SaveSystem.Load();
        if (data != null)
        {
            ApplyState(data);
        }
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
        settingsPanel.ForceClose();

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

    public void LoadScene(string name)
    {
        Time.timeScale = 1f;
        SaveNow();
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// Ghi state hiện tại xuống file save, bỏ qua khi game đang dừng hoặc player đang bị khoá điều khiển.
    /// </summary>
    public void SaveNow()
    {
        if (isReloadingScene)
        {
            return;
        }

        if (!CanSave())
        {
            return;
        }

        GameSaveData data = CaptureState();
        if (data != null)
        {
            SaveSystem.Save(data);
        }
    }

    /// <summary>
    /// Kết thúc game thắng: xoá save và chuyển sang màn EndGame.
    /// </summary>
    public void CompleteGame()
    {
        isReloadingScene = true;
        Time.timeScale = 1f;
        SaveSystem.Delete();
        SceneManager.LoadScene("EndGame");
    }

    /// <summary>
    /// Player chết: nạp lại scene hiện tại để save gần nhất tự áp dụng ở Start.
    /// </summary>
    public void OnPlayerDied()
    {
        if (isReloadingScene)
        {
            return;
        }

        isReloadingScene = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnApplicationQuit()
    {
        SaveNow();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveNow();
        }
    }

    private bool CanSave()
    {
        if (Time.timeScale == 0f)
        {
            return false;
        }

        if (HackManager.Instance != null && HackManager.Instance.IsActive)
        {
            return false;
        }

        if (playerMovement != null && !playerMovement.enabled)
        {
            return false;
        }

        if (inputManager != null && !inputManager.enabled)
        {
            return false;
        }

        if (playerLook != null && !playerLook.enabled)
        {
            return false;
        }

        return playerTransform != null
            && playerHealth != null
            && playerLook != null
            && gunHolder != null
            && documentManager != null;
    }

    /// <summary>
    /// Gom toàn bộ state cần lưu của scene gameplay thành một GameSaveData.
    /// </summary>
    private GameSaveData CaptureState()
    {
        GameSaveData data = new GameSaveData
        {
            saveVersion = CurrentSaveVersion,
            playerPosition = playerTransform.position,
            playerYaw = playerTransform.eulerAngles.y,
            cameraPitch = playerLook.CameraPitch,
            playerHealth = playerHealth.CurrentHealth,
            currentWeapon = gunHolder.currentWeapon,
            countdownRemaining = -1f
        };

        GameObject[] weapons = gunHolder.weapons;
        int weaponCount = weapons != null ? weapons.Length : 0;

        data.gunUnlocked = new bool[weaponCount];
        data.gunBulletsLeft = new int[weaponCount];
        data.gunAmmoReserve = new int[weaponCount];

        for (int i = 0; i < weaponCount; i++)
        {
            Gun gun = weapons[i] != null ? weapons[i].GetComponent<Gun>() : null;
            if (gun == null)
            {
                continue;
            }

            data.gunUnlocked[i] = gun.isPlayable;
            data.gunBulletsLeft[i] = gun.bulletsLeft;
            data.gunAmmoReserve[i] = gun.amountOfBullet;
        }

        List<DocumentData> documents = documentManager.documents;
        List<string> documentTitles = new List<string>();
        if (documents != null)
        {
            for (int i = 0; i < documents.Count; i++)
            {
                if (documents[i] != null)
                {
                    documentTitles.Add(documents[i].title);
                }
            }
        }
        data.collectedDocuments = documentTitles.ToArray();

        List<string> openedIds = new List<string>();
        Interactable[] interactables = FindObjectsOfType<Interactable>(true);

        for (int i = 0; i < interactables.Length; i++)
        {
            Interactable interactable = interactables[i];

            if (interactable is Crate crate)
            {
                if (crate.IsSolved)
                {
                    openedIds.Add(crate.SaveId);
                }
            }
            else if (interactable is Locket locket)
            {
                if (locket.IsOpen)
                {
                    openedIds.Add(locket.SaveId);
                }
            }
            else if (interactable is Lever lever)
            {
                if (lever.IsUsed)
                {
                    data.leverUsed = true;
                    openedIds.Add(lever.SaveId);
                }
            }
        }
        data.openedObjectIds = openedIds.ToArray();

        if (countdownTimer != null && countdownTimer.IsRunning)
        {
            data.countdownRemaining = countdownTimer.Remaining;
        }

        return data;
    }

    /// <summary>
    /// Khôi phục scene gameplay về đúng state đã lưu.
    /// </summary>
    private void ApplyState(GameSaveData data)
    {
        if (playerTransform == null || playerHealth == null || playerLook == null || gunHolder == null)
        {
            Debug.LogError("GameManager: thiếu reference nên không thể áp dụng save.", this);
            return;
        }

        RestorePlayer(data);
        RestoreWeapons(data);
        RestoreInteractables(data);
    }

    private void RestorePlayer(GameSaveData data)
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        playerTransform.position = data.playerPosition;
        playerTransform.rotation = Quaternion.Euler(0f, data.playerYaw, 0f);

        if (playerController != null)
        {
            playerController.enabled = true;
        }

        playerLook.CameraPitch = data.cameraPitch;
        playerHealth.SetHealth(data.playerHealth);
    }

    private void RestoreWeapons(GameSaveData data)
    {
        GameObject[] weapons = gunHolder.weapons;
        if (weapons == null)
        {
            return;
        }

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
            {
                continue;
            }

            Gun gun = weapons[i].GetComponent<Gun>();
            if (gun == null)
            {
                continue;
            }

            if (data.gunUnlocked != null && i < data.gunUnlocked.Length)
            {
                gun.isPlayable = data.gunUnlocked[i];
            }

            if (data.gunBulletsLeft != null && i < data.gunBulletsLeft.Length)
            {
                gun.bulletsLeft = data.gunBulletsLeft[i];
            }

            if (data.gunAmmoReserve != null && i < data.gunAmmoReserve.Length)
            {
                gun.amountOfBullet = data.gunAmmoReserve[i];
            }
        }

        if (data.currentWeapon >= 0 && data.currentWeapon < weapons.Length)
        {
            gunHolder.SelectWeapon(data.currentWeapon);
        }
    }

    private void RestoreInteractables(GameSaveData data)
    {
        HashSet<string> openedIds = new HashSet<string>();
        if (data.openedObjectIds != null)
        {
            for (int i = 0; i < data.openedObjectIds.Length; i++)
            {
                openedIds.Add(data.openedObjectIds[i]);
            }
        }

        HashSet<string> collectedDocuments = new HashSet<string>();
        if (data.collectedDocuments != null)
        {
            for (int i = 0; i < data.collectedDocuments.Length; i++)
            {
                collectedDocuments.Add(data.collectedDocuments[i]);
            }
        }

        Interactable[] interactables = FindObjectsOfType<Interactable>(true);

        for (int i = 0; i < interactables.Length; i++)
        {
            Interactable interactable = interactables[i];

            if (interactable is Crate crate)
            {
                if (openedIds.Contains(crate.SaveId))
                {
                    crate.ForceSolved();
                }
            }
            else if (interactable is Locket locket)
            {
                if (openedIds.Contains(locket.SaveId))
                {
                    locket.ForceOpen();
                }
            }
            else if (interactable is Lever lever)
            {
                if (data.leverUsed || openedIds.Contains(lever.SaveId))
                {
                    lever.ForceUsed();
                }
            }
            else if (interactable is DocumentInterac documentPickup)
            {
                if (documentPickup.document != null &&
                    collectedDocuments.Contains(documentPickup.document.title))
                {
                    if (documentManager != null)
                    {
                        documentManager.AddDocument(documentPickup.document);
                    }
                    Destroy(documentPickup.gameObject);
                }
            }
            else if (interactable is GunInteract gunPickup)
            {
                if (gunPickup.gunToUnlock != null && gunPickup.gunToUnlock.isPlayable)
                {
                    Destroy(gunPickup.gameObject);
                }
            }
        }

        if (countdownTimer != null && data.countdownRemaining > 0f)
        {
            countdownTimer.ResumeCountdown(data.countdownRemaining);
        }
    }

    private void ValidateSaveReferences()
    {
        if (playerTransform == null)
        {
            Debug.LogError("GameManager: chưa gán playerTransform trong Inspector.", this);
        }

        if (playerHealth == null)
        {
            Debug.LogError("GameManager: chưa gán playerHealth trong Inspector.", this);
        }

        if (playerLook == null)
        {
            Debug.LogError("GameManager: chưa gán playerLook trong Inspector.", this);
        }

        if (gunHolder == null)
        {
            Debug.LogError("GameManager: chưa gán gunHolder trong Inspector.", this);
        }

        if (documentManager == null)
        {
            Debug.LogError("GameManager: chưa gán documentManager trong Inspector.", this);
        }

        if (countdownTimer == null)
        {
            Debug.LogError("GameManager: chưa gán countdownTimer trong Inspector.", this);
        }

        if (playerMovement == null)
        {
            Debug.LogError("GameManager: chưa gán playerMovement trong Inspector.", this);
        }

        if (inputManager == null)
        {
            Debug.LogError("GameManager: chưa gán inputManager trong Inspector.", this);
        }
    }
}
