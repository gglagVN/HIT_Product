using Thnguyet.AudioManagement;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource voiceSource;

    [SerializeField]
    private AudioSource footstepSource;

    [Header("Zombie")]
    public AudioClipGroup idleClips = new AudioClipGroup();
    public AudioClipGroup detectClips = new AudioClipGroup();
    public AudioClipGroup attackClips = new AudioClipGroup();
    public AudioClipGroup hurtClips = new AudioClipGroup();
    public AudioClipGroup deathClips = new AudioClipGroup();
    public AudioClipGroup walkFootsteps = new AudioClipGroup();
    public AudioClipGroup runFootsteps = new AudioClipGroup();

    [Header("Gunner")]
    public AudioClipGroup gunnerShootClips = new AudioClipGroup();
    public AudioClipGroup gunnerHurtClips = new AudioClipGroup();
    public AudioClipGroup gunnerDeathClips = new AudioClipGroup();
    public AudioClipGroup gunnerWalkFootsteps = new AudioClipGroup();
    public AudioClipGroup gunnerRunFootsteps = new AudioClipGroup();

    [Header("Settings")]
    [Range(0.8f, 1.2f)]
    public float minPitch = 0.95f;

    [Range(0.8f, 1.2f)]
    public float maxPitch = 1.05f;



    private void PlayRandom(AudioClipGroup group)
    {
        if (group == null)
            return;

        AudioClip clip = group.GetNextClip();

        if (clip == null)
            return;

        voiceSource.pitch = Random.Range(minPitch, maxPitch);

        voiceSource.PlayOneShot(clip);

        voiceSource.pitch = 1f;
    }

    /// Lấy clip cố định dùng cho footstep loop.
    private AudioClip GetLoopClip(AudioClipGroup group)
    {
        if (group == null || group.audioClips == null || group.audioClips.Length == 0)
            return null;

        return group.audioClips[0];
    }

    public void StartWalkLoop()
    {
        AudioClip loopClip = GetLoopClip(walkFootsteps);

        if (loopClip == null)
            return;

        if (footstepSource.isPlaying &&
            footstepSource.clip == loopClip)
            return;

        footstepSource.Stop();

        footstepSource.clip = loopClip;

        footstepSource.loop = true;

        footstepSource.Play();
    }

    public void StartRunLoop()
    {
        AudioClip loopClip = GetLoopClip(runFootsteps);

        if (loopClip == null)
            return;

        if (footstepSource.isPlaying &&
            footstepSource.clip == loopClip)
            return;

        footstepSource.Stop();

        footstepSource.clip = loopClip;

        footstepSource.loop = true;

        footstepSource.Play();
    }

    public void StopFootstep()
    {
        if (!footstepSource.isPlaying)
            return;

        footstepSource.Stop();
    }

    //================ ZOMBIE =================

    public void PlayIdle()
    {
        PlayRandom(idleClips);
    }

    public void PlayDetect()
    {
        PlayRandom(detectClips);
    }

    public void PlayAttack()
    {
        PlayRandom(attackClips);
    }

    public void PlayHurt()
    {
        PlayRandom(hurtClips);
    }

    public void PlayDeath()
    {
        PlayRandom(deathClips);
    }

    public void PlayWalkFootstep()
    {
        PlayRandom(walkFootsteps);
    }

    public void PlayRunFootstep()
    {
        PlayRandom(runFootsteps);
    }

    //================ GUNNER =================

    public void PlayGunnerShoot()
    {
        PlayRandom(gunnerShootClips);
    }

    public void PlayGunnerHurt()
    {
        PlayRandom(gunnerHurtClips);
    }

    public void PlayGunnerDeath()
    {
        PlayRandom(gunnerDeathClips);
    }

    public void PlayGunnerWalk()
    {
        PlayRandom(gunnerWalkFootsteps);
    }

    public void PlayGunnerRun()
    {
        PlayRandom(gunnerRunFootsteps);
    }
}
