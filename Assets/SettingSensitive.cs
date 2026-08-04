using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingSensitive : MonoBehaviour
{
    [Header("Normal Sensitivity")]
    public TextMeshProUGUI normalText;
    public Slider sliderNormal;

    [Header("ADS Sensitivity")]
    public TextMeshProUGUI ADSText;
    public Slider sliderADS;

    [Header("Player")]
    public PlayerLook playerLook;

    private void Start()
    {
        // Load giá trị đã lưu
        float normalValue =
            PlayerPrefs.GetFloat(
                "NormalSensitivity",
                100f
            );

        float adsValue =
            PlayerPrefs.GetFloat(
                "ADSSensitivity",
                100f
            );

        // Gán cho Slider
        sliderNormal.value = normalValue;
        sliderADS.value = adsValue;

        // Hiển thị
        UpdateNormalText(normalValue);
        UpdateADSText(adsValue);

        // Áp dụng vào PlayerLook
        if (playerLook != null)
        {
            playerLook.SetNormalSensitivity(normalValue);
            playerLook.SetADSSensitivity(adsValue);
        }
    }

    // =========================
    // NORMAL SENSITIVITY
    // =========================

    public void SetNormalSensitivity(float value)
    {
        PlayerPrefs.SetFloat(
            "NormalSensitivity",
            value
        );

        UpdateNormalText(value);

        if (playerLook != null)
        {
            playerLook.SetNormalSensitivity(value);
        }
    }

    // =========================
    // ADS SENSITIVITY
    // =========================

    public void SetADSSensitivity(float value)
    {
        PlayerPrefs.SetFloat(
            "ADSSensitivity",
            value
        );

        UpdateADSText(value);

        if (playerLook != null)
        {
            playerLook.SetADSSensitivity(value);
        }
    }

    // =========================
    // TEXT
    // =========================

    private void UpdateNormalText(float value)
    {
        normalText.text =
            Mathf.RoundToInt(value) + "%";
    }

    private void UpdateADSText(float value)
    {
        ADSText.text =
            Mathf.RoundToInt(value) + "%";
    }
}