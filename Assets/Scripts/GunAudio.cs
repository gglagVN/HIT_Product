using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GunAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] public AudioClip shootClip;
    public AudioClip reloadClip;
    public AudioClip emptyClip;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayShoot()
    {
        if (shootClip != null)
            audioSource.PlayOneShot(shootClip);
    }

    public void PlayReload()
    {
        if (reloadClip != null)
            audioSource.PlayOneShot(reloadClip);
    }

    public void PlayEmpty()
    {
        if (emptyClip != null)
            audioSource.PlayOneShot(emptyClip);
    }
}