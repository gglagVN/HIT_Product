using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsAudio : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Sliders")]
    public Slider masterSlider;
    public TextMeshProUGUI masterText;
    public Slider musicSlider;
    public TextMeshProUGUI musicText;
    public Slider sfxSlider;
    public TextMeshProUGUI sfxText;
    public Slider voiceSlider;
    public TextMeshProUGUI voiceText;
    public Slider ambientSlider;
    public TextMeshProUGUI ambientText;

    void Start()
    {
        LoadSettings();
    }

    // =========================
    // MASTER
    // =========================

    public void SetMasterVolume(float value)
    {
        SetVolume("MasterVolume", value);
        masterText.text = Mathf.RoundToInt(value * 100) + "%";
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    // =========================
    // MUSIC
    // =========================

    public void SetMusicVolume(float value)
    {
        SetVolume("MusicVolume", value);
        musicText.text = Mathf.RoundToInt(value * 100) + "%";
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    // =========================
    // SFX
    // =========================

    public void SetSFXVolume(float value)
    {
        SetVolume("SFXVolume", value);
        sfxText.text = Mathf.RoundToInt(value * 100) + "%";
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    // =========================
    // VOICE
    // =========================

    public void SetVoiceVolume(float value)
    {
        SetVolume("VoiceVolume", value);
        voiceText.text = Mathf.RoundToInt(value * 100) + "%";
        PlayerPrefs.SetFloat("VoiceVolume", value);
    }

    // =========================
    // AMBIENT
    // =========================

    public void SetAmbientVolume(float value)
    {
        SetVolume("AmbientVolume", value);
        ambientText.text = Mathf.RoundToInt(value * 100) + "%";
        PlayerPrefs.SetFloat("AmbientVolume", value);
    }

    // =========================
    // APPLY VOLUME
    // =========================

    private void SetVolume(string parameter, float value)
    {
        // Slider từ 0 -> 1
        // AudioMixer dùng dB

        if (value <= 0.0001f)
        {
            audioMixer.SetFloat(parameter, -80f);
        }
        else
        {
            float dB = Mathf.Log10(value) * 20f;
            audioMixer.SetFloat(parameter, dB);
        }
    }

    // =========================
    // LOAD
    // =========================

    void LoadSettings()
    {
        float master =
            PlayerPrefs.GetFloat("MasterVolume", 1f);

        float music =
            PlayerPrefs.GetFloat("MusicVolume", 1f);

        float sfx =
            PlayerPrefs.GetFloat("SFXVolume", 1f);

        float voice =
            PlayerPrefs.GetFloat("VoiceVolume", 1f);

        float ambient =
            PlayerPrefs.GetFloat("AmbientVolume", 1f);


        masterSlider.value = master;
        musicSlider.value = music;
        sfxSlider.value = sfx;
        voiceSlider.value = voice;
        ambientSlider.value = ambient;


        SetVolume("MasterVolume", master);
        SetVolume("MusicVolume", music);
        SetVolume("SFXVolume", sfx);
        SetVolume("VoiceVolume", voice);
        SetVolume("AmbientVolume", ambient);
    }
}