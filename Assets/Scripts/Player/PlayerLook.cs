using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;

    private float xRotation = 0f;

    public float CameraPitch
    {
        get => xRotation;
        set
        {
            xRotation = Mathf.Clamp(value, -80f, 80f);

            if (cam != null)
            {
                cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            }
        }
    }

    [Header("Normal Sensitivity")]
    public float xSensitive = 20f;
    public float ySensitive = 20f;

    [Header("ADS")]
    public float adsXSensitive = 10f;
    public float adsYSensitive = 10f;

    private bool canLook = true;

    // Sensitivity gốc
    private float baseXSensitive;
    private float baseYSensitive;

    // ADS gốc
    private float baseADSXSensitive;
    private float baseADSYSensitive;

    // % sensitivity hiện tại
    private float normalSensitivityPercent;
    private float adsSensitivityPercent;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        // Lưu giá trị mặc định trong Inspector
        baseXSensitive = xSensitive;
        baseYSensitive = ySensitive;

        baseADSXSensitive = adsXSensitive;
        baseADSYSensitive = adsYSensitive;

        // Load setting
        normalSensitivityPercent =
            PlayerPrefs.GetFloat("NormalSensitivity", 100f);

        adsSensitivityPercent =
            PlayerPrefs.GetFloat("ADSSensitivity", 100f);

        ApplyNormalSensitivity();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (!canLook)
            return;

        if (GameManager.Instance != null &&
            GameManager.Instance.IsPaused())
            return;
        if (playerHealth.isDead == true)
            return;

        // Click chuột để khóa lại
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // =========================
    // ENABLE / DISABLE LOOK
    // =========================

    public void SetLookEnabled(bool value)
    {
        canLook = value;
    }

    // =========================
    // NORMAL SENSITIVITY
    // =========================

    public void SetNormalSensitivity(float percent)
    {
        normalSensitivityPercent = percent;

        PlayerPrefs.SetFloat(
            "NormalSensitivity",
            normalSensitivityPercent
        );

        // Nếu đang không ADS thì áp dụng ngay
        ApplyNormalSensitivity();
    }

    private void ApplyNormalSensitivity()
    {
        float multiplier = normalSensitivityPercent / 100f;

        xSensitive = baseXSensitive * multiplier;
        ySensitive = baseYSensitive * multiplier;
    }

    // =========================
    // ADS
    // =========================

    public void SetADSSensitivity(float percent)
    {
        adsSensitivityPercent = percent;

        PlayerPrefs.SetFloat(
            "ADSSensitivity",
            adsSensitivityPercent
        );

        ApplyADSSensitivity();
    }

    private void ApplyADSSensitivity()
    {
        float multiplier = adsSensitivityPercent / 100f;

        adsXSensitive = baseADSXSensitive * multiplier;
        adsYSensitive = baseADSYSensitive * multiplier;
    }

    // =========================
    // ADS STATE
    // =========================

    public void SetADS(bool value)
    {
        if (value)
        {
            xSensitive = adsXSensitive;
            ySensitive = adsYSensitive;
        }
        else
        {
            ApplyNormalSensitivity();
        }
    }

    // =========================
    // LOOK
    // =========================

    public void ProcessLook(Vector2 input)
    {
        if (!canLook)
            return;

        if (GameManager.Instance != null &&
            GameManager.Instance.IsPaused())
            return;

        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= mouseY *
                     Time.deltaTime *
                     ySensitive;

        xRotation = Mathf.Clamp(
            xRotation,
            -80f,
            80f
        );

        cam.transform.localRotation =
            Quaternion.Euler(
                xRotation,
                0,
                0
            );

        transform.Rotate(
            Vector3.up *
            mouseX *
            Time.deltaTime *
            xSensitive
        );
    }
}